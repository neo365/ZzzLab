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
                                alert("알수없는 에러");
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
                                //          접근 거부 된거나 DNS 문제 등으로 접속 안되는 주소로 연결 했을때.
                                // case 2 : ajax 호출하고 반환값을 받아와야 되는데, 이 반환값이 도착 하기전에 submit 또는 새로고침 등으로 페이지 이동이 발생해 버리는 경우.
                                //
                                // == 일단은 어떤 케이스 인지 모르니 무시하고 로그에만 찍자.
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
                                    alert('[400] 잘못된 요청입니다. (Bad Request)');
                                }
                                break;

                            // 인증통과 실패 : RFC6750 참조
                            case 401: // 권한 없음
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[401] 페이지 접근권한이 없습니다. (Unauthorized) ');
                                }
                                break;

                            case 402: // 결제 필요
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[402] 이 요청은 결제가 필요합니다. (Payment Required) ');
                                }
                                break;

                            // 인증은 성공했으나 해당 리소스에 대한 접근 권한이 없음.
                            case 403: // 금지됨
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[403] 사용자가 리소스에 대한 필요 권한을 갖고 있지 않습니다. (Forbidden)');
                                }
                                break;

                            case 404: // 찾을 수 없음
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[404] 서버가 요청한 페이지를 찾을 수 없습니다. (Not Found)');
                                }
                                break;

                            case 405: // Method가 잘못됨.
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[405] 요청이 잘못되었습니다. (Method Not Allowed)');
                                }
                                break;

                            case 408: // 요청시간이 초과 되었습니다.
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[408] 요청시간이 초과 되었습니다. (Request Timeout)');
                                }
                                break;

                            case 500: // 서버에러
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[500] 서버에 오류가 발생하여 요청을 수행할 수 없습니다. (Internal server error.)');
                                }
                                break;

                            case 501: // 구현되지 않음
                                if (xhr.responseJSON && xhr.responseJSON.message) {
                                    alert(xhr.responseJSON.message);
                                    if (xhr.responseJSON.returnUrl) {
                                        location.replace(xhr.responseJSON.returnUrl);
                                    }
                                }
                                else {
                                    alert('[501] 서버에 요청을 수행할 수 있는 기능이 없습니다. (Internal server error.)');
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
                                    alert('[503] 서버가 오버로드되었거나 유지관리를 위해 다운되었기 때문에 현재 서버를 사용할 수 없습니다. (Service unavailable.)');
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
            emptyTable: "데이터가 없습니다.",
            info: "전체 _TOTAL_ 개",//"Showing _START_ to _END_ of _TOTAL_ 개의 항목",
            infoEmpty: "",//"Showing 0 to 0 of 0 entries",
            infoFiltered: "(총 _MAX_ 개)", //"(filtered from _MAX_ total entries)",
            infoPostFix: "",
            decimal: ",",
            thousands: ",",
            lengthMenu: '<span class="dataTables_lengthMenu">페이지당 줄수</span> _MENU_', //Show _MENU_ entries,
            loadingRecords: "읽는중...",
            processing: "처리중...", //"Processing...",
            search: '<span class="dataTables_search">검색</span> : ', //"Search:",
            zeroRecords: "검색 결과가 없습니다", //"No matching records found",
            paginate: {
                first: "처음", //"First",
                last: "마지막",//"Last",
                next: "다음",//"Next",
                previous: "이전" //"Previous"
            },
            aria: {
                sortAscending: ": 오름차순 정렬", //": activate to sort column ascending",
                sortDescending: ": 내림차순 정렬", //": activate to sort column descending"
            }
        }
    });
}));