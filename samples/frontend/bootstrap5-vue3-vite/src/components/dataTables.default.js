/**
 * @summary     ColResize
 * @description Provide the ability to resize columns in a DataTable
 * @version     1.6.1
 * @file        jquery.dataTables.colResize.js
 * @author      Daniel Hobi, Lado Tadic, Daniel Petras
 * Language:    Javascript
 * License:     MIT
 */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery', 'datatables.net'], function ($) {
            return factory($, window, document);
        });
    }
    else if (typeof exports === 'object') {
        // CommonJS
        module.exports = function (root, $) {
            if (!root) {
                root = window;
            }

            if (!$ || !$.fn.dataTable) {
                $ = require('datatables.net')(root, $).$;
            }

            return factory($, root, root.document);
        };
    }
    else {
        // Browser
        factory(jQuery, window, document);
    }
}(function ($, window, document) {
    'use strict';
    function settingsFallback(userSetting, fallBackSetting) {
        let resultObject = {};
        for (let prop in fallBackSetting) {
            if (!fallBackSetting.hasOwnProperty(prop)) {
                continue;
            }
            if (userSetting.hasOwnProperty(prop)) {
                let userObject = userSetting[prop];
                if (typeof userObject === 'object') {
                    resultObject[prop] = settingsFallback(userObject, fallBackSetting[prop]);
                } else {
                    resultObject[prop] = userObject;
                }
            } else {
                resultObject[prop] = fallBackSetting[prop];
            }
        }
        return resultObject;
    }

    let DataTable = $.fn.dataTable;

    $.extend($.fn.dataTable.defaults, {
        //searching: true,
        //searchDelay: 1000,
        //ordering: true,
        //paging: true,
        //pageLength: 10,
        //lengthMenu: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
        //pagingType: "first_last_numbers",
        //bAutoWidth: true,
        //serverSide: true,
        processing: true,
        //stateSave: true,
        //stateDuration: 60 * 60 * 24, // 1Day
        //stateDuration: 60 * 1, // 1 Min
        //stateDuration: -1, // Sesstion Time
        ajax: {
            headers: {
                'RequestVerificationToken': $('meta[name="requestVerificationToken"]').attr('content'),
                'Authorization': 'Bearer ' + $.cookie("accessToken"),
                'AccessToken': $.cookie("accessToken"),
                'UserId': btoa(unescape(encodeURIComponent($.cookie("userId"))))
            },
            timeout: 1000 * 60 * 3, // sets timeout to 3 min
            type: 'POST',
            dataType: "json",
            params: function (grid, setting) {
                return null;
            },
            data: function (grid) {
                try {
                    var param = {
                        uuid: $.cookie("uuid"),
                        locale: $.cookie("locale"),
                        userId: $.cookie("user_id"),
                        apKkey: $.cookie("api_key"),
                        accessToken:$.cookie("accessToken"),
                        actionKey: 'r',
                        start: grid.start,
                        limit: grid.length,
                        searchText: (grid.search ? grid.search.value : null),
                        searchDate: $("#search_date").val(),
                        columns: grid.columns,
                        orderColumn: function () {
                            if (grid.order && grid.order.length > 0) {
                                return grid.columns[grid.order[0].column].name;
                            }

                            return '';
                        },
                        orderDir: function () {
                            if (grid.order && grid.order.length > 0) {
                                return grid.order[0].dir;
                            }

                            return 'asc';
                        }
                    };

                    if (this.params) {
                        if (typeof (this.params) == 'function') {
                            var appendParam = params(grid, setting);

                            if (appendParam && typeof (appendParam) === 'object') {
                                $.extend(param, appendParam);
                            }
                        }
                    }

                    $.each(param.columns, function (index, item) {
                        if (item.searchable) this.search = grid.search;
                    });

                    return param;
                }
                catch (e) {
                    alert(e.message);
                }
            },
            complete: function (resp) {
                try {
                    if (resp.responseJSON) {
                        var response = resp.responseJSON;

                        if (response && response.result == false) {
                            if (response.message) {
                                alert(response.message);
                            }
                            else {
                                alert("???????????? ??????");
                            }
                        }
                    }
                }
                catch (e) {
                    alert(e.message);
                }
            },
            error: function (xhr, exception, thrownError) {
                try {
                    if (!!xhr.message) {
                        alert("Error : " + xhr.message);
                    }
                    else if (xhr.status && xhr.status != 200) {
                        switch (xhr.status) {
                            case 0:
                                // case 1 : In my experience, you'll see a status of 0 when either doing cross-site scripting (where access is denied) or requesting a URL that is unreachable (typo, DNS issues, etc).
                                //          ?????? ?????? ????????? DNS ?????? ????????? ?????? ????????? ????????? ?????? ?????????.
                                // case 2 : ajax ???????????? ???????????? ???????????? ?????????, ??? ???????????? ?????? ???????????? submit ?????? ???????????? ????????? ????????? ????????? ????????? ????????? ??????.
                                //
                                // == ????????? ?????? ????????? ?????? ????????? ???????????? ???????????? ??????.
                                console.log('[0] Not connect. Verify Network.');
                                break;

                            case 400:
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[400] ????????? ???????????????. (Bad Request)');
                                }
                                break;

                            // ???????????? ?????? : RFC6750 ??????
                            case 401: // ?????? ??????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[401] ????????? ??????????????? ????????????. (Unauthorized) ');
                                }
                                break;

                            case 402: // ?????? ??????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[402] ??? ????????? ????????? ???????????????. (Payment Required) ');
                                }
                                break;

                            // ????????? ??????????????? ?????? ???????????? ?????? ?????? ????????? ??????.
                            case 403: // ?????????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[403] ???????????? ???????????? ?????? ?????? ????????? ?????? ?????? ????????????. (Forbidden)');
                                }
                                break;

                            case 404: // ?????? ??? ??????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[404] ????????? ????????? ???????????? ?????? ??? ????????????. (Not Found)');
                                }
                                break;

                            case 405: // Method??? ?????????.
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[405] ????????? ?????????????????????. (Method Not Allowed)');
                                }
                                break;

                            case 408: // ??????????????? ?????? ???????????????.
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[408] ??????????????? ?????? ???????????????. (Request Timeout)');
                                }
                                break;

                            case 500: // ????????????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[500] ????????? ????????? ???????????? ????????? ????????? ??? ????????????. (Internal server error.)');
                                }
                                break;

                            case 501: // ???????????? ??????
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[501] ????????? ????????? ????????? ??? ?????? ????????? ????????????. (Internal server error.)');
                                }
                                break;

                            case 503:
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[503] ????????? ???????????????????????? ??????????????? ?????? ??????????????? ????????? ?????? ????????? ????????? ??? ????????????. (Service unavailable.)');
                                }
                                break;

                            default:
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[' + xhr.status + ']' + xhr.statusText);
                                }
                                break;
                        }
                    }
                    else if (exception) {
                        switch (exception) {
                            case 'parsererror':
                                alert('[Failed]Requested JSON parse failed.');
                                break;

                            case 'timeout':
                                alert('[Timeout]Time out error. ');
                                break;

                            case 'abort': // log only
                                console.log('[Aborted]Ajax request aborted.');
                                //alert('[Aborted]Ajax request aborted.');
                                break;

                            default:  // log only
                                console.log('Uncaught Error.n' + xhr.responseText);
                                //alert('Uncaught Error.n' + xhr.responseText);
                                break;
                        }
                    }
                    else {
                        alert("Error : " + thrownError);
                    }
                }
                catch (e) {
                    alert(e.message);
                }
            }
        },
        language: {
            emptyTable: "???????????? ????????????.",
            info: "?????? _TOTAL_ ???",//"Showing _START_ to _END_ of _TOTAL_ ?????? ??????",
            infoEmpty: "",//"Showing 0 to 0 of 0 entries",
            infoFiltered: "(??? _MAX_ ???)", //"(filtered from _MAX_ total entries)",
            infoPostFix: "",
            decimal: ",",
            thousands: ",",
            lengthMenu: '<span class="dataTables_lengthMenu">???????????? ??????</span> _MENU_', //Show _MENU_ entries,
            loadingRecords: "?????????...",
            processing: "?????????...", //"Processing...",
            search: '<span class="dataTables_search">??????</span> : ', //"Search:",
            zeroRecords: "?????? ????????? ????????????", //"No matching records found",
            paginate: {
                first: "??????", //"First",
                last: "?????????",//"Last",
                next: "??????",//"Next",
                previous: "??????" //"Previous"
            },
            aria: {
                sortAscending: ": ???????????? ??????", //": activate to sort column ascending",
                sortDescending: ": ???????????? ??????", //": activate to sort column descending"
            }
        }
    });
}));