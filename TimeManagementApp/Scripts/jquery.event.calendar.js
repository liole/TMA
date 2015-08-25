/*
 *  jQuery Event Calendar 1.0
 *  Demo's and documentation:
 *	ecalendar.ozkanozturk.me
 *
 *	Copyright © 2014 Özkan Öztürk
 *	www.ozkanozturk.me
 */

(function($) {

	$.fn.eCalendar = function( options ) {

		var date			= new Date(),
			currentMonth	= date.getMonth() + 1,
			currentYear		= date.getFullYear();

		var el				= this,
			defaults 		= {
				eventsContainer		: '#hb-event-list',														// container that contains the event list
				ajaxDayLoader		: 'ajax/hb-days.php',													// php file that returns event days of current month
				ajaxEventLoader		: 'ajax/hb-events.php',													// php file that returns event list of current day
				currentMonth		: currentMonth,															// default current month
				currentYear			: currentYear,															// default current year
				startMonth			: currentMonth,															// month of min. date (default current month)
				startYear			: currentYear,															// year of min. date (default current year)
				endMonth			: currentMonth,															// month of max. date (default current month of next year)
				endYear				: currentYear + 1,														// year of max. date (default next year)
				firstDayOfWeek		: 1,																	// fisrt day of the week, 0: Sunday, 1: Monday (default)
				onBeforeLoad		: function() {},														// event: before eCalendar starts loading
				onAfterLoad			: function() {},														// event: after eCalendar loaded
				onClickMonth		: function() {},														// event: click on month name
				onClickDay			: function() {},														// event: click on an event day
			},
			options					= $.extend( {}, defaults, options );


		/* initalize elements and view */
		this.init = function() {

			options.onBeforeLoad();

			if( !el.hasClass('hb-calendar') ) {

				//el.attr('class', 'hb-calendar');
				el.addClass('hb-calendar');

			}

			if( el.find('.hb-months').length == 0 && el.find('.hb-days').length == 0 ) {

				el.html('<div class="hb-months">' +
							'<a href="javascript:;" class="hb-change-month hb-prev-month" data-month="" data-year=""></a>' +
							'<a href="javascript:;" class="hb-current-month" data-month="" data-year=""></span>' +
							'<a href="javascript:;" class="hb-change-month hb-next-month " data-month="" data-year=""></a>' +
						'</div>' +
						'<div class="hb-days"></div>' );

			}

			if( options.eventsContainer == '#hb-event-list' && el.next('#hb-event-list').length == 0 ) {

				el.after( '<div id="hb-event-list"></div>');

			}

			options.eventDays	= getEventDays( options.ajaxDayLoader, options.currentMonth, options.currentYear );			// getting current month's event days

			this.initMonths();
			this.initDays();

			options.onAfterLoad();

			this.find('.hb-months a.hb-change-month').unbind('click').bind('click', function() {

				//options.onClickMonth();

				var opts = {
						currentMonth	: $(this).attr('data-month'),
						currentYear		: $(this).attr('data-year')
					},
					opts = $.extend( {}, options, opts );

				el.eCalendar( opts );

			});
			
			this.find('.hb-months a.hb-current-month').unbind('click').bind('click', function() {
			
				$('.hb-day-selected').removeClass('hb-day-selected');
				options.onClickMonth(options.currentMonth, options.currentYear);
				
			});

		}


		/* initalize months */
		this.initMonths = function() {

			var monthsWrapper	= el.find('.hb-months');

			// Previous Month
			var prevMonth = parseInt(options.currentMonth) - 1,
				prevYear  = parseInt(options.currentYear);

			if( prevMonth == 0 ) {
				prevMonth = 12;
				prevYear  = prevYear - 1;
			}

			if( prevYear < options.startYear ) {

				monthsWrapper.find('.hb-prev-month').css( 'display', 'none' );

			}else{

				if( prevMonth < options.startMonth && prevYear == options.startYear ) {

					monthsWrapper.find('.hb-prev-month').css( 'display', 'none' );

				}else{

					monthsWrapper.find('.hb-prev-month').css( 'display', '' );
					monthsWrapper.find('.hb-prev-month').attr( 'data-month', prevMonth );
					monthsWrapper.find('.hb-prev-month').attr( 'data-year', prevYear );

				}

			}

			// Current Month
			monthsWrapper.find('.hb-current-month').attr( 'data-month', options.currentMonth );
			monthsWrapper.find('.hb-current-month').attr( 'data-year',  options.currentYear );
			monthsWrapper.find('.hb-current-month').html( months[options.currentMonth - 1] + '<span>' + options.currentYear + '</span>' );

			// Next Month
			var nextMonth = parseInt(options.currentMonth) + 1,
				nextYear  = parseInt(options.currentYear);

			if( nextMonth == 13 ) {
				nextMonth = 1;
				nextYear  = nextYear + 1;
			}

			if( nextYear > options.endYear ) {

				monthsWrapper.find('.hb-next-month').css( 'display', 'none' );

			}else{

				if( nextMonth > options.endMonth && nextYear == options.endYear ) {

					monthsWrapper.find('.hb-next-month').css( 'display', 'none' );

				}else{

					monthsWrapper.find('.hb-next-month').css( 'display', '' );
					monthsWrapper.find('.hb-next-month').attr( 'data-month', nextMonth );
					monthsWrapper.find('.hb-next-month').attr( 'data-year', nextYear );

				}

			}

		}


		/* initalize days */
		this.initDays = function() {

			var daysWrapper	= el.find('.hb-days');
			var dayCount	= new Date(options.currentYear, options.currentMonth, 0).getDate();
			var calendar	= '',
				dayIndex	= 0,
				dayNumber	= 1,
				dayClass	= '',
				dayData		= '',
				cDate		= '',
				firstDay;

			if( options.firstDayOfWeek == 1 ) {

				// Monday is first day
				firstDay	= new Date(options.currentYear, options.currentMonth - 1, 1).getDay();

				for( var i = 0; i < 7; i++ )
					calendar += '<span href="javascript:;" class="hb-day hb-day-name">' + days[i] + '</span>';

			}else{

				// Sunday is first day
				firstDay  = new Date(options.currentYear, options.currentMonth - 1, 2).getDay();
				calendar += '<span href="javascript:;" class="hb-day">' + days[6] + '</span>';

				for( var i = 0; i < 6; i++ ) {
					calendar += '<span href="javascript:;" class="hb-day hb-day-name">' + days[i] + '</span>';
				}

			}

			if( firstDay == 0 )
				firstDay = 7;

			for( var i = 1; i < dayCount + firstDay; i++ ) {

				dayIndex = (i % 7);

				if( i < firstDay )

					calendar += '<span href="javascript:;" class="hb-day">&nbsp;</span>';

				else{

					if( inArray( dayNumber, options.eventDays ) ) {

						cDate = dayNumber.toString() + options.currentMonth.toString() + options.currentYear.toString();

						if ( cDate == $('body').data('activeDate') ) {

							dayClass = "hb-day hb-day-active hb-day-selected";

						}else{

							dayClass = "hb-day hb-day-active";

						}

						dayData	= ' data-day="' + dayNumber + '"';

					}else{

						dayClass	= "hb-day";
						dayData		= "";

					}

					calendar += '<span href="javascript:;" class="' + dayClass + '" ' + dayData + '>' + dayNumber + '</span>';

					dayNumber++;

				}

			}

			daysWrapper.html( calendar );

		}


		/* CLICK on event day */
		el.on('click', '.hb-day-active', function() {

			var e				= $(this),
				currentDay		= e.attr('data-day'),
				currentMonth	= $('body').find('.hb-current-month').attr('data-month'),
				currentYear		= $('body').find('.hb-current-month').attr('data-year');

			if( !e.hasClass('hb-day-selected') ) {

				options.onClickDay(currentDay, currentMonth, currentYear);

				$('.hb-day-selected').removeClass('hb-day-selected');
				e.addClass('hb-day-selected');

				// SELECT active event list of current day from db
				/*
				$.ajax({
					url			: options.ajaxEventLoader,
					type		: "POST",
					async		: false,
					data		: {
						currentDay		: currentDay,
						currentMonth	: currentMonth,
						currentYear		: currentYear
					},
					beforeSend	: function() {
						$(options.eventsContainer).html('');
						$(options.eventsContainer).addClass('hb-loading');
					},
					success		: function(result) {
						$('body').data( 'activeDate', currentDay.toString() + currentMonth.toString() + currentYear.toString() );
						$(options.eventsContainer).html(result);
					},
					error		: function() {
						$(options.eventsContainer).html('<div class="hb-error">' + errorMessage + '</div>');
					},
					complete	: function() {
						$(options.eventsContainer).removeClass('hb-loading');
					}
				});
				*/

			}

		});


		/* SELECT active event days of current month from db */
		function getEventDays(url, currentMonth, currentYear) {

			var ajaxData = $.ajax({
				url		: url,
				type	: "GET",
				async	: false,
				data	: {
					currentMonth: currentMonth,
					currentYear: currentYear
				}
			});

			var daysList = JSON.parse(ajaxData.responseText);//.split(',');

			return daysList;

		}


		/* DETECT active event days in month */
		function inArray(needle, haystack) {

			var length = haystack.length;

			for(var i = 0; i < length; i++)
				if(haystack[i] == needle) return true;

			return false;

		}

		this.init();

	}


	// Methods
	/** /
	$.fn.eCalendar.function = function() {}
	/**/


})(jQuery);
