// 아쉬운 데로 edge 라도 전환하자
const agent = window.navigator.userAgent.toLowerCase();
const appName = window.navigator.appName.toLowerCase();
const isIE = ((appName == 'netscape' && agent.search('trident') != -1) || (agent.indexOf("msie") != -1));

if(isIE) {
    if(confirm("현재 페이지는 인터넷익스플로러(IE)브라우저를 지원하지 않습니다. " + "크롬 혹은 엣지브라우저에서 정상적으로 작동됩니다. 해당 브라우저로 이동하시겠습니까?")) {
        window.location = "microsoft-edge:" + window.location.href;
    }
}