if (typeof window.console == 'undefined') {
    window.console = {
        log: function () {
        }
    };
}

if (!String.prototype.parseDate) {
    String.prototype.parseDate = function () {
        var b = this.split(/\D+/);
        // Month is zero-based
        return new Date(b[0], Number(b[1]) - 1, b[2], b[3], b[4], b[5]);
    };
}

if (!String.prototype.parseISODate) {
    String.prototype.parseISODate = function () {
        var b = this.split(/\D+/);
        return new Date(Date.UTC(b[0], --b[1], b[2], b[3], b[4], b[5], b[6]));
    };
}

if (!Date.prototype.toLocalDate) {
    Date.prototype.toLocalDate = function () {
        var newDate = new Date(this.getTime() + this.getTimezoneOffset() * 60 * 1000);

        var offset = this.getTimezoneOffset() / 60;
        var hours = this.getHours();

        newDate.setHours(hours - offset);

        return newDate;
    };
}

if (!Date.prototype.toShortDateString) {
    Date.prototype.toShortDateString = function () {
        var yyyy = this.getFullYear().toString();
        var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = this.getDate().toString();

        return yyyy + '-' + (mm[1] ? mm : "0" + mm[0]) + '-' + (dd[1] ? dd : "0" + dd[0]);
    };
}

if (!Date.prototype.toTimeString) {
    Date.prototype.toTimeString = function () {
        var HH = this.getHours().toString();
        var mm = this.getMinutes().toString();
        var ss = this.getSeconds().toString();

        return (HH[1] ? HH : "0" + HH[0]) + ':' + (mm[1] ? mm : "0" + mm[0]) + ':' + (ss[1] ? ss : "0" + ss[0]);
    };
}

if (!Date.prototype.to24DateString) {
    Date.prototype.to24DateString = function () {
        var HH = this.getHours().toString();
        var mm = this.getMinutes().toString();
        var ss = this.getSeconds().toString();

        return this.toShortDateString() + ' ' + (HH[1] ? HH : "0" + HH[0]) + ':' + (mm[1] ? mm : "0" + mm[0]) + ':' + (ss[1] ? ss : "0" + ss[0]);
    };
}

if (!Date.prototype.Yesterday) {
    Date.prototype.Yesterday = function () {
        return new Date(this.valueOf() - (24 * 60 * 60 * 1000));
    };
}

if (!Date.prototype.Tomorrow) {
    Date.prototype.Tomorrow = function () {
        return new Date(this.valueOf() + (24 * 60 * 60 * 1000));
    };
}

if (!Date.prototype.formatDefault) {
    Date.prototype.formatDefault = function () {
        return [
            this.getFullYear().toString(),
            (this.getMonth() + 1).toString().replace(/^(\d)$/, '0$1'),
            (this.getDate()).toString().replace(/^(\d)$/, '0$1')
        ].join('-');
    };
}

if (!Date.prototype.isValid) {
    Date.prototype.isValid = function () {
        return this.getTime() === this.getTime();
    };
}

if (!Date.prototype.addDays) {
    Date.prototype.addDays = function (days) {
        var dat = new Date(this.valueOf());
        dat.setDate(dat.getDate() + days);
        return dat;
    };
}

if (!String.prototype.toDate) {
    String.prototype.toDate = function () {
        var year = this.substr(0, 4);
        var month = this.substr(5, 2);
        var day = this.substr(8, 2);

        return new Date(year, (month - 1), day);
    };
}

/*
 * Array에 비어있는 요소를 제거하는 기능을 추가
 */
if (!Array.prototype.trimIndex) {
    Array.prototype.trimIndex = function () {
        for (var i = this.length - 1, j = 0; i >= j; i--) {
            if (typeof this[i] == 'undefined')
                this.splice(i, 1);
        }
    };
}

/*
 * Array에 비어있는 요소를 제거하는 기능을 추가
 */
if (!Array.prototype.deleteEmptyIndex) {
    Array.prototype.deleteEmptyIndex = function () {
        for (var i = this.length - 1, j = 0; i >= j; i--) {
            if (typeof this[i] == 'undefined')
                this.splice(i, 1);
        }
    };
}

if (!String.prototype.replaceAll) {
    String.prototype.replaceAll = function (org, dest) {
        return this.split(org).join(dest);
    };
}

if (!String.prototype.startsWith) {
    String.prototype.startsWith = function (searchString, position) {
        position = position || 0;
        return this.indexOf(searchString, position) === position;
    };
}

if (!String.prototype.endsWith) {
    // endwith 기능 추가
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

if (!String.prototype.isNumber) {
    // isNumber 기능 추가
    String.prototype.isNumber = function () {
        var value = this;
        var result = true;
        for (var j = 0; result && (j < value.length); j++) {
            if ((value.substring(j, j + 1) < "0") || (value.substring(j, j + 1) > "9")) {
                result = false;
            }
        }
        return result;
    };
}

if (!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/(^\s*)|(\s*$)/gi, "");
    }
}