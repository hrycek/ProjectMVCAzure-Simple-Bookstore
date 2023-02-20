let x = document.cookie;
console.log("x:" + x);
let element = document.getElementById("navLoginDiv");

function myFunction() {
    console.log("set cookie expired");
    document.cookie = "currUser=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    location.reload();
}

document.getElementById("buttonLogout").addEventListener("click", myFunction);


if (x.startsWith("currUser=")){
    document.getElementById("navLoginLink").innerHTML = "Wyloguj się"
    document.getElementById("navLoginDiv").style.display = "none";
    document.getElementById("buttonLogout").style.display = "";
    if (x.startsWith("currUser=admin")) {
        document.getElementById("navSupportPanelLink").style.display = ""
    } else {
        document.getElementById("navSupportPanelLink").style.display = "none"
    }
    console.log(x);
} else {
    document.getElementById("navLoginLink").innerHTML = "Logowanie"
    document.getElementById("navLoginDiv").style.display = "";
    document.getElementById("buttonLogout").style.display = "none";
    document.getElementById("navSupportPanelLink").style.display = "none"
    console.log(x);
}