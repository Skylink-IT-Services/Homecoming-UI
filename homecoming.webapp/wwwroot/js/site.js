// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function AddItem(btn) {
    var table = document.getElementById('infoTable');
    var rows = table.getElementsByTagName('tr');

    var rowOuterHtml = rows[rows.length - 1].outerHTML;
    var lastrowIndex = document.getElementById('hdnLastIndex').value;
    var nextrowIndex = eval(lastrowIndex) + 1;
    document.getElementById('hdnLastIndex').value = nextrowIndex;

    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIndex + '_', '_' + nextrowIndex + '_');
    rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIndex + ']', '[' + nextrowIndex + ']');
    rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIndex, '_' + nextrowIndex);

    var newRow = table.insertRow();
    newRow.innerHTML = rowOuterHtml;


    var btnAddID = btn.id;
    var btnDeleteId = btnAddID.replaceAll('btnadd', 'btnremove');

    var delBtn = document.getElementById(btnDeleteId);
    delBtn.classList.add("visible");
    delBtn.classList.remove("invisible");

    var addBtn = document.getElementById(btnAddID);
    addBtn.classList.remove("visible");
    addBtn.classList.add("invisible");
}

function DeleteItem(btn) {
    $(btn).closest('tr').remove();
}