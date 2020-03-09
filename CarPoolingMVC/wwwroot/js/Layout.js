function SetNavBar()
{
    if (location.pathname == "/" || location.pathname == "/user/login" || location.pathname.toLowerCase() == "/user/signup") {
        $("nav").hide();
    }
    else {
        $("nav").show();        
    }
}