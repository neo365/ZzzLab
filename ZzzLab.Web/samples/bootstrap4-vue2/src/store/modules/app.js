import Cookies from 'js-cookie'

const app = {
    state: {
        sidebar: {
            opened: true,
            withoutAnimation: false,
        },
        device: 'desktop',
        // language: Cookies.get('language') || 'kr',
        language: 'kr',
        size: Cookies.get('size') || 'medium',
    },
    mutations: {
        TOGGLE_SIDEBAR: state => {
            state.sidebar.opened = !state.sidebar.opened;
            state.sidebar.withoutAnimation = false;
            if (state.sidebar.opened) {
                Cookies.set('sidebarStatus', 1);
                //document.getElementById('menu-hidden').style.left = '190px';
                document.getElementById('menu-hidden').style.backgroundImage = "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACEAAAAhCAYAAABX5MJvAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RDNEODc5QzhENjZEMTFFQ0JGRTdCODJFMTlFNTVDMEYiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RDNEODc5QzlENjZEMTFFQ0JGRTdCODJFMTlFNTVDMEYiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpEM0Q4NzlDNkQ2NkQxMUVDQkZFN0I4MkUxOUU1NUMwRiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpEM0Q4NzlDN0Q2NkQxMUVDQkZFN0I4MkUxOUU1NUMwRiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PiQA7k8AAABNSURBVHja7NVBCgAgCAXRvP+hp61bAxFsPIC8+ooBnOkKESJEiNiGeNGGcaxHkHJmYiaoNO74Caqv617R+H4mXFFvh3GIECFCRK4rwABnyGu/fvvHMgAAAABJRU5ErkJggg==')";

                document.getElementById('menu-hidden').style.left = '0';
                //document.getElementById('menu-hidden').style.backgroundImage = "url('../assets/images/leftmenu-close.png')";
            } else {
                Cookies.set('sidebarStatus', 0);
                //document.getElementById('menu-hidden').style.left = '35.2px';
                document.getElementById('menu-hidden').style.backgroundImage = "url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACEAAAAhCAYAAABX5MJvAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6REYxNzZGMjJENjZEMTFFQzk1OTZFQzQyMUY1QzcwRTciIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6REYxNzZGMjNENjZEMTFFQzk1OTZFQzQyMUY1QzcwRTciPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpERjE3NkYyMEQ2NkQxMUVDOTU5NkVDNDIxRjVDNzBFNyIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpERjE3NkYyMUQ2NkQxMUVDOTU5NkVDNDIxRjVDNzBFNyIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PkSwlJ0AAABLSURBVHja7NUxEgAgCANB8/9Hx5YWG0DO2mInRJHtU30EAgQIED8hXqRiHOs7oXBX1Um4QzGdfSGjkhjXCf4JdgedAAECxA7EFWAA07lrv+a/DAEAAAAASUVORK5CYII=')";
                document.getElementById('menu-hidden').style.left = '0';
                //document.getElementById('menu-hidden').style.backgroundImage = "url('../assets/images/leftmenu-open.png')";
            }
        },
        CLOSE_SIDEBAR: (state, withoutAnimation) => {
            Cookies.set('sidebarStatus', 0);
            state.sidebar.opened = false;
            state.sidebar.withoutAnimation = withoutAnimation;
        },
        TOGGLE_DEVICE: (state, device) => {
            if (device === 'mobile') {
                document.getElementById('menu-hidden').style.display = '';
            } else {
                document.getElementById('menu-hidden').style.display = '';
            }
            state.device = device;
        },
        SET_LANGUAGE: (state, language) => {
            state.language = language;
            Cookies.set('language', language);
        },
        SET_SIZE: (state, size) => {
            state.size = size;
            Cookies.set('size', size);
        },
    },
    actions: {
        toggleSideBar({commit}) {
            commit('TOGGLE_SIDEBAR');
        },
        closeSideBar({commit}, {withoutAnimation}) {
            commit('CLOSE_SIDEBAR', withoutAnimation);
        },
        toggleDevice({commit}, device) {
            commit('TOGGLE_DEVICE', device);
        },
        setLanguage({commit}, language) {
            commit('SET_LANGUAGE', language);
        },
        setSize({commit}, size) {
            commit('SET_SIZE', size);
        },
    },
};

export default app;