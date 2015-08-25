function toggleMenu()
{
    $("nav.menu").toggleClass("open");
}
function showLoginInput(obj) {
    $(obj).hide();
    $("#loginInput").show();
}
function backToFirst()
{
    $('section').removeClass("inFocuse");
    $('.first').addClass("inFocuse");
}
function toggleCalendar()
{
    if ($('#calendarBlock2').is(':empty'))
        $('#calendarBlock2').eCalendar(document.calendarOptions);
    $('#calendarBlock2').toggleClass("open");
}
function caseEmploee(n)
{
    if (n % 10 == 1) return "працівник";
    if (n % 10 > 1 && n % 10 < 5) return "працівники";
    if (n % 10 >= 5 || n % 10 == 0) return "працівників";
}
function caseProject(n) {
    if (n % 10 == 1) return "проект";
    if (n % 10 > 1 && n % 10 < 5) return "проекти";
    if (n % 10 >= 5 || n % 10 == 0) return "проектів";
}

function leadingZero(num)
{
    return ((num < 10) ? "0" : "") + num;
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
function removeCookie(cname) {
    setCookie(cname, "", -1);
}
function UTC(date) {
    return new Date(date);
}

var timeApp = angular.module('timeApp', ['ngRoute']);

timeApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'Views/welcome.html',
            controller: 'welcomeController'
        })
        .when('/Auth', {
            templateUrl: 'Views/login.html',
            controller: 'loginController'
        })
        .when('/Auth/User', {
            templateUrl: 'Views/login.html',
            controller: 'loginUserController'
        })
        .when('/Auth/Company', {
            templateUrl: 'Views/login.html',
            controller: 'loginCompanyController'
        })
        .when('/Auth/SignOut', {
            templateUrl: 'Views/login.html',
            controller: 'signOutController'
        })
        .when('/Company/Projects', {
            templateUrl: 'Views/company_projects.html',
            controller: 'companyProjectsController'
        })
        .when('/Company/Emploees', {
            templateUrl: 'Views/emploees.html',
            controller: 'companyEmploeesController'
        })
        .when('/User/Projects', {
            templateUrl: 'Views/user_projects.html',
            controller: 'userProjectsController'
        })
        .when('/User/Profile', {
            templateUrl: 'Views/user_profile.html',
            controller: 'userProfileController'
        })
});

document.menus = {
    "unauthorized": [
        {
            "title": "Вхід",
            "href": "Auth",
            "id": "login"
        }
    ],
    "Company": [
        {
            "title": "Проекти",
            "href": "Company/Projects",
            "id": "c_projects"
        },
        {
            "title": "Працівники",
            "href": "Company/Emploees",
            "id": "emploees"
        },
        {
            "title": "Профіль",
            "href": "Company/Profile",
            "id": "c_profile"
        },
        {
            "title": "Вихід",
            "href": "Auth/SignOut",
            "id": "logout"
        }
    ],
    "User": [
        {
            "title": "Проекти",
            "href": "User/Projects",
            "id": "u_projects"
        },
        {
            "title": "Профіль",
            "href": "User/Profile",
            "id": "u_profile"
        },
        {
            "title": "Вихід",
            "href": "Auth/SignOut",
            "id": "logout"
        }
    ]
}

timeApp.directive('defaultSrc', function () {
    var defaultSrc = {
        link: function postLink(scope, iElement, iAttrs) {
            iElement.bind('error', function () {
                angular.element(this).attr("src", iAttrs.defaultSrc);
            });
        }
    }
    return defaultSrc;
});

timeApp.directive('optionsColor', function ($parse) {
    return {
        require: 'select',
        link: function (scope, elem, attrs, ngSelect) {
            var optionsSourceStr = attrs.ngOptions.split(' ').pop();
            scope.$watch(optionsSourceStr, function (items) {
                angular.forEach(items, function (item, index) {
                    option = elem.find('option[value=' + index + ']');
                    angular.element(option).css("background-color",item.Color);
                });
            });
        }
    };
});

function reloadMenu ()
{
    $("#menuList").empty();
    document.menus[document.currentUser.Type].forEach(function (item) {
        $("#menuList").append('<li id="'+item.id+'"><a href="#'+item.href+'">'+item.title+'</a></li>');
    });
    selectMenuItem(document.selectedMenuItem);
}

function initialize($http)
{
    document.token = getCookie("token");
    if (document.token == "") {
        document.currentUser = {};
        document.currentUser.Type = "unauthorized";
        reloadMenu();
    } else {
        $http.get('/api/Auth/' + document.token)
            .success(function (data) {
                document.currentUser = data;
                $http.defaults.headers.common["AuthToken"] = document.token;
                $.ajaxSetup({ headers: { AuthToken: document.token } });
                reloadMenu();
                if (document.dataReady != undefined)
                    document.dataReady();
            })
            .error(function (data, status) {
                if (status == 404) {
                    document.currentUser = {};
                    document.currentUser.Type = "unauthorized";
                    reloadMenu();
                    removeCookie("token");
                    document.token = "";
                }
            });
    }
}

function selectMenuItem(id) {
    document.selectedMenuItem = id;
    $("nav.menu").removeClass("open");
    if (document.currentUser != undefined) {
        $("#menuList li").removeClass("selected");
        $("#menuList #" + id).addClass("selected");
    }
}
/*
function defineLoginControllerMethods(name, $scope, $http)
{
    $scope.displayInfo = function () {
        $http.get('/api/'+name+'/Info/' + $scope.login)
            .success(function (data) {
                $scope.preview = data;
                $("."+name+"LoginInfo").show();
                $("#loginInput").hide();
                $("#loginInput").removeClass("invalid");
            })
            .error(function (data, status) {
                if (status == 404)
                    $("#loginInput").addClass("invalid");
            });
    }
    $scope.tryLogIn = function () {
        $http.post('/api/Auth/'+name, { login: $scope.login, password: $scope.password })
            .success(function (data) {
                data = data.substring(1, data.length - 1);
                setCookie("token", data, 30);
                document.token = data;
                $http.get('/api/Auth/' + data)
                    .success(function (data) {
                        document.currentUser = data;
                        $http.defaults.headers.common["AuthToken"] = document.token;
                        reloadMenu();
                        location.hash = name + "/Projects";
                    });
            })
            .error(function (data, status) {
                if (status == 404)
                    $("#loginInput").addClass("invalid");
                if (status == 401)
                    $("#passwordInput").addClass("invalid");
            });
    }
}
*/
function whenReady (func) {
    if (document.currentUser != undefined)
        func()
    else document.dataReady = func;
}

timeApp.controller('initializeController', function ($scope, $http) {
    initialize($http);
});

timeApp.controller('welcomeController', function ($scope) {
    //selectMenuItem("welcome");
    $scope.message = 'Welcome to this appliaction!';
});
timeApp.controller('loginController', function ($scope, $http) {
    selectMenuItem("login");
    $scope.caseEmploee = caseEmploee;
    $scope.title = 'Вхід';

    $scope.displayInfo = function () {
        $http.get('/api/Auth/Type/' + $scope.login)
            .success(function (data) {
                type = data.substring(1, data.length - 1);
                $http.get('/api/' + type + '/Info/' + $scope.login)
                    .success(function (data) {
                        $scope.preview = data;
                        $("." + type + "LoginInfo").show();
                        $("#loginInput").hide();
                        $("#loginInput").removeClass("invalid");
                    })
            })
            .error(function (data, status) {
                if (status == 404)
                    $("#loginInput").addClass("invalid");
            });
    }
    $scope.tryLogIn = function () {
        $http.post('/api/Auth', { login: $scope.login, password: $scope.password })
            .success(function (data) {
                data = data.substring(1, data.length - 1);
                setCookie("token", data, 30);
                document.token = data;
                $http.get('/api/Auth/' + data)
                    .success(function (data) {
                        document.currentUser = data;
                        $http.defaults.headers.common["AuthToken"] = document.token;
                        $.ajaxSetup({ headers: { AuthToken: document.token } });
                        reloadMenu();
                        location.hash = data.Type + "/Projects";
                    });
            })
            .error(function (data, status) {
                if (status == 404)
                    $("#loginInput").addClass("invalid");
                if (status == 401)
                    $("#passwordInput").addClass("invalid");
            });
    }
});
/*
timeApp.controller('loginUserController', function ($scope, $http) {
    selectMenuItem("login");
    $scope.title = 'Вхід для користувачів';
    $scope.changeTitle = 'для компаній';
    $scope.changeLink = 'Company';
    defineLoginControllerMethods("User", $scope, $http);
});
timeApp.controller('loginCompanyController', function ($scope, $http) {
    selectMenuItem("login");
    $scope.caseNoun = caseEmploee;
    $scope.title = 'Вхід для компаній';
    $scope.changeTitle = 'для користувачів';
    $scope.changeLink = 'User';
    defineLoginControllerMethods("Company", $scope, $http);
});
*/
timeApp.controller('signOutController', function ($scope, $http) {
    selectMenuItem("signOut");
    $scope.title = 'Виконується вихід';
    $scope.changeTitle = '. . .';
    $http.delete('/api/Auth/' + document.token)
            .success(function (data) {
                removeCookie("token");
                document.token = "";
                document.currentUser.Type = "unauthorized";
                reloadMenu();
                location.hash = "Auth";
            });
});

function defineRecordsFilters($scope, $http, userDescr, callback)
{
    $scope.recordsFilter = {
        full: true,
        append: false,
        user: userDescr,
        year: 0,
        month: 0,
        day: 0,
        start: 0,
        count: 3,
        moreAvialable: true
    };
    $scope.reloadRecords = function () {
        if ($scope.recordsFilter.append == false)
            $scope.recordsFilter.start = 0;
        var uri = 'api/Project/' + $scope.selected + '/User/' + $scope.recordsFilter.user + '/Records';
        if (!$scope.recordsFilter.full)
            uri += '/' + $scope.recordsFilter.year + '/' + $scope.recordsFilter.month + '/' + $scope.recordsFilter.day;
        uri += '/' + $scope.recordsFilter.start + '/' + $scope.recordsFilter.count;
        $http.get(uri)
        .success(function (data) {
            $scope.recordsFilter.moreAvialable = data.length == $scope.recordsFilter.count;
            if ($scope.recordsFilter.append)
                data = $scope.records.concat(data);
            $scope.records = data;
            $('#recentActivity .blockTooltip').hide();
            $('section').removeClass("inFocuse");
            $('#recentActivity').addClass("inFocuse");
        });
    };
    $scope.reloadRecords();

    $scope.reloadCalendarOptions = function (callback) {
        $http.get("api/Project/" + $scope.selected + "/User/" + $scope.recordsFilter.user + "/Records/Days/Limits")
            .success(function (data) {
                var min = new Date(data.min);
                var max = new Date(data.max);
                var now = new Date();
                var currMonth = now.getMonth() + 1;
                var currYear = now.getFullYear();
                if (now > max) {
                    cuurMonth = max.getMonth() + 1;
                    currYear = max.getFullYear();
                }
                document.calendarOptions = {
                    ajaxDayLoader: "api/Project/" + $scope.selected + "/User/" + $scope.recordsFilter.user + "/Records/Days",
                    startMonth: min.getMonth() + 1,
                    startYear: min.getFullYear(),
                    endMonth: max.getMonth() + 1,
                    endYear: min.getFullYear(),
                    currentMonth: currMonth,
                    currentYear: currYear,
                    firstDayOfWeek: 1,
                    onClickMonth: function (m, y) {
                        $.extend($scope.recordsFilter, {
                            full: false,
                            append: false,
                            day: 0,
                            month: m,
                            year: y
                        });
                        $scope.reloadRecords();
                    },
                    onClickDay: function (d, m, y) {
                        $.extend($scope.recordsFilter, {
                            full: false,
                            append: false,
                            day: d,
                            month: m,
                            year: y
                        });
                        $scope.reloadRecords();
                    }
                };
                callback();
            });
    }
    $scope.reloadCalendarOptions(callback);
    $scope.loadMoreRecords = function () {
        $.extend($scope.recordsFilter, {
            append: true,
            start: $scope.recordsFilter.start + $scope.recordsFilter.count
        });
        $scope.reloadRecords();
    }
}

timeApp.controller('companyProjectsController', function ($scope, $http) {
    selectMenuItem("c_projects");
    whenReady(function () {
        $http.get('api/Company/' + document.currentUser.Data.ID + "/Projects")
            .success(function (data) {
                $scope.projects = data;
            });
    });
    $scope.UTC = UTC;
    $scope.duration = function (beg, end) {
        var time = UTC(end) - UTC(beg);
        var h = ~~(time / (60 * 60 * 1000));
        var m = ~~((time % (60 * 60 * 1000)) / (60 * 1000));
        var res = "";
        if (h != 0)
            res += h + " год ";
        if (m != 0)
            res += m + " хв";
        return res;
    }
    $scope.displayDetails = function (id) {
        $scope.selected = id;
        $scope.selectedUser = null;
        defineRecordsFilters($scope, $http, "All", function () {
            $('#calendarBlock2').eCalendar(document.calendarOptions)
        });
        /*
        $http.get('api/Project/'+id+'/Records/0/10/+User')
            .success(function (data) {
                $scope.records = data;
                $('#recentActivity .blockTooltip').hide();
                $('section').removeClass("inFocuse");
                $('#recentActivity').addClass("inFocuse");
            });
        */
        $scope.caseNoun = caseProject;
        $scope.getHoursLZ = function (hours) { return leadingZero(~~hours); };
        $scope.getMinutesLZ = function (hours) { return leadingZero(~~((hours * 60) % 60)); };
        $scope.getHours = function (hours) { return ~~hours; };
        $scope.getMinutes = function (hours) { return ~~((hours * 60) % 60); };
        $http.get('api/Project/' + id + '/Users')
            .success(function (data) {
                $scope.users = data;
                $('#users .blockTooltip').hide();
            });
    }
    $scope.loadUserRecords = function (userID) {
        $.extend($scope.recordsFilter, {
            user: userID,
            full: true,
            append: false
        });
        $scope.reloadCalendarOptions(function () {
            $('#calendarBlock2').eCalendar(document.calendarOptions);
        });
        $scope.selectedUser = userID;
        $scope.reloadRecords();
    }
    $scope.loadUserStatistics = function (user) {
        if (!user.tagStatistics)
            $http.get('api/User/' + user.ID + '/Project/' + $scope.selected + '/TagStatistic')
               .success(function (data) {
                   user.tagStatistics = [];
                   data.forEach(function (tag) {
                       if (tag.UserHours > 0)
                           user.tagStatistics.push(tag);
                   })
                   //user.tagStatistics = data;
               });
        else user.tagStatistics = null;
    }
});
function createDefaultRecord($scope, $http, callback)
{
    var now = new Date();
    var record = {
        BeginTime: "",
        EndTime: leadingZero(now.getHours()) + ":" + leadingZero(now.getMinutes()),
        Date: leadingZero(now.getDate()) + "." + leadingZero(now.getMonth() + 1) + "." + now.getFullYear(),
        Description: "",
        Tag: {}
    };
    if (!$scope.tags) {
        $http.get('api/RecordTag/List')
        .success(function (data) {
            $scope.tags = data;
            record.Tag = $scope.tags[1];
            callback(record);
        });
    } else {
        record.Tag = $scope.tags[1];
        callback(record);
    }
}
timeApp.controller('userProjectsController', function ($scope, $http) {
    selectMenuItem("u_projects");
    whenReady(function () {
        $http.get('api/User/' + document.currentUser.Data.ID + "/Projects")
            .success(function (data) {
                $scope.projects = data;
            });
    });
    $scope.UTC = UTC;
    $scope.duration = function (beg, end) {
        var time = UTC(end) - UTC(beg);
        var h = ~~(time / (60 * 60 * 1000));
        var m = ~~((time % (60 * 60 * 1000)) / (60 * 1000));
        var res = "";
        if (h != 0)
            res += h + " год ";
        if (m != 0)
            res += m + " хв";
        return res;
    }
    $scope.getHours = function (hours) { return leadingZero(~~hours); };
    $scope.getMinutes = function (hours) { return leadingZero(~~((hours * 60) % 60)); };

    $scope.displayDetails = function (id) {
        $scope.selected = id;
        
        /*
        $scope.tags = [
            { ID: 1, Text: "reading", Color: "rgb(221, 238, 255)" },
            { ID: 2, Text: "coding", Color: "rgb(238, 255, 221)" },
            { ID: 3, Text: "designing", Color: "rgb(255, 255, 204)" }
        ];
        */
        createDefaultRecord($scope, $http, function (data) {
            $scope.newRecord = data;
        });
        defineRecordsFilters($scope, $http, "Me", function () {
            $('#calendarBlock').eCalendar(document.calendarOptions)
        });
        $('#calendar .blockTooltip').hide();
        $scope.addRecord = function () {
            var date = $scope.newRecord.Date.split('.');
            var begTime = $scope.newRecord.BeginTime.split(':');
            var endTime = $scope.newRecord.EndTime.split(':');
            var record = {
                BeginTime: new Date(date[2], ~~date[1] - 1, date[0], begTime[0], begTime[1], 0, 0).toISOString(),
                EndTime: new Date(date[2], ~~date[1] - 1, date[0], endTime[0], endTime[1], 0, 0).toISOString(),
                Description: $scope.newRecord.Description,
                Status: 0,
                UserID: document.currentUser.Data.ID,
                ProjectID: $scope.selected,
                TagID: $scope.newRecord.Tag.ID
            };
            $http.post('/api/Record', record)
            .success(function (data) {
                $scope.records = [data].concat($scope.records);
                createDefaultRecord($scope, $http, function (data) {
                    $scope.newRecord = data;
                });
            });
        };
        $scope.startRecord = function () {
            var record = {
                BeginTime: new Date().toISOString(),
                EndTime: new Date().toISOString(),
                Description: $scope.newRecord.Description,
                Status: 1,
                UserID: document.currentUser.Data.ID,
                ProjectID: $scope.selected,
                TagID: $scope.newRecord.Tag.ID
            };
            $http.post('/api/Record', record)
            .success(function (data) {
                $scope.records = [data].concat($scope.records);
                createDefaultRecord($scope, $http, function (data) {
                    $scope.newRecord = data;
                });
            });
        }
        $scope.stopRecord = function (record) {
            $http.put('/api/Record/' + record.ID + "/Finish")
            .success(function (data) {
                record.Status = 0;
                record.EndTime = data.EndTime;
            });
        }
    }
});
timeApp.controller('userProfileController', function ($scope, $http) {
    selectMenuItem("u_profile");
    whenReady(function () {
        $scope.user = document.currentUser.Data;
        $scope.saveSuccessful = false;
        $http.get('api/User/' + document.currentUser.Data.ID + '/TagStatistic')
            .success(function (data) {
                $scope.tagStatistics = data;
                $scope.totalHours = 0;
                for (var i = 0; i < data.length; ++i)
                    $scope.totalHours += data[i].UserHours;
            });
    });
    $scope.getHours = function (hours) { return ~~hours; };
    $scope.getMinutes = function (hours) { return ~~((hours * 60) % 60); };
    $scope.saveUserData = function () {
        if ($scope.user.Password != $scope.user.RepeatPassword && $scope.user.Password != "") {
            $(".userPasswordRepeat").addClass("invalid");
        } else {
            $(".userPasswordRepeat").removeClass("invalid");
            var userData = {
                FullName: $scope.user.FullName,
                Password: $scope.user.Password
            }
            $http.put('api/User/' + document.currentUser.Data.ID, userData)
            .success(function (data) {
                $scope.saveSuccessful = true;
                document.currentUser.Data.FullName = $scope.user.FullName;
            });
        }
    }

});
timeApp.controller('companyEmploeesController', function ($scope, $http) {
    selectMenuItem("emploees");
    whenReady(function () {
        $http.get('api/RecordTag/List')
        .success(function (data) {
            $scope.tags = data;
        });
        $http.get('api/User/TagStatistic')
       .success(function (data) {
           $scope.users = data;
       });
    });
    $scope.getHoursLZ = function (hours) { return leadingZero(~~hours); };
    $scope.getMinutesLZ = function (hours) { return leadingZero(~~((hours * 60) % 60)); };
    $scope.getHours = function (hours) { return ~~hours; };
    $scope.getMinutes = function (hours) { return ~~((hours * 60) % 60); };
    $scope.userSortingIndex = -1;
    $scope.userSortingDecending = true;
    $scope.hideChecked = false;
    $scope.userSorting = function () {
        var res = '';
        if ($scope.userSortingIndex == -2)
            res = 'ProjectsCount';
        else if ($scope.userSortingIndex == -1)
            res = 'TotalHours';
        else res = 'Tags[' + $scope.userSortingIndex + '].UserHours';
        //if ($scope.userSortingDecending)
        //    res = '-'+res;
        return res;
    }
    $scope.toggleSorting = function(index) {
        if ($scope.userSortingIndex == index)
            $scope.userSortingDecending = !$scope.userSortingDecending;
        else {
            $scope.userSortingIndex = index;
            $scope.userSortingDecending = true;
        }
    }
    $scope.toggleHide = function () {
        $scope.search = null;
        $scope.hideChecked = !$scope.hideChecked;
    }
    $scope.selectEmploee = function (user) {
        $('section').removeClass("inFocuse");
        $('#editEmploee').addClass("inFocuse");
        $scope.selected = user;
        $('#editEmploee .blockTooltip').hide();
    }
    $scope.caseNoun = caseProject;
    $scope.saveUserData = function () {
        if ($scope.selected.Password != $scope.selected.RepeatPassword && $scope.selected.Password != "") {
            $(".userPasswordRepeat").addClass("invalid");
        } else {
            $(".userPasswordRepeat").removeClass("invalid");
            var userData = {
                FullName: $scope.selected.FullName,
                Password: $scope.selected.Password
            }
            $http.put('api/User/' + document.currentUser.Data.ID, userData)
            .success(function (data) {
                $scope.saveSuccessful = true;
                document.currentUser.Data.FullName = $scope.user.FullName;
            });
        }
    }
});