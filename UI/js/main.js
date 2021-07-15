document.getElementById("myDiv").onclick = function () {
    document.getElementsByClassName("wrap-box-modal")[0].style.display = "flex";
}

document.getElementById("exit-modal").onclick = function (){
    document.getElementsByClassName("wrap-box-modal")[0].style.display = "none";
}