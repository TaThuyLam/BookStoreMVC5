function SearchByColumn(num) {
    var filter, td, i, txtValue;
    filter = input.value.toUpperCase();


    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[num];
        if (td) {
            txtValue = td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}