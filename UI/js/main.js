document.getElementById("myDiv").onclick = function () {
    document.getElementsByClassName("wrap-box-modal")[0].style.display = "flex";
    document.getElementById("firstField").focus();
    getNewCode();
}

document.getElementById("exit-modal").onclick = function (){
    document.getElementsByClassName("wrap-box-modal")[0].style.display = "none";
}

//Search Input
$(document).ready(function() {
    $('.input-search').on('keyup', function(event) {
       event.preventDefault();
       /* Act on the event */
       var tukhoa = $(this).val().toLowerCase();
       $('table tbody tr').filter(function() {
          $(this).toggle($(this).text().toLowerCase().indexOf(tukhoa)>-1);
       });
    });
 });

//Click button refresh
$(document).ready(function(){
    $('.refresh-button').on('click', function(){
        $('table tbody tr').hide();
        loadData();
    })
})