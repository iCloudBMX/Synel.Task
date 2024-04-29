function DeleteEmployeeById(id) {
    if (window.confirm("Do you really want to delete?"))
        window.location = "/delete/" + id;
}

function Search() {
    let searchText = document.getElementById("search").value;

    if (!IsEmpty(searchText)) {

        let filters = [
            {
                "Property": "FirstName",
                "Comparison": "=",
                "Value": searchText,
            }
        ];

        let url = new URL(window.location);

        for (let index = 0; index < filters.length; index++) {
            let filter = filters[index];
            url.searchParams.set(`parameters.Filters[${index}].Property`, filter.Property);
            url.searchParams.set(`parameters.Filters[${index}].Comparison`, filter.Comparison);
            url.searchParams.set(`parameters.Filters[${index}].Value`, filter.Value);
        }

        window.location = url.toString();
    } else {
        let url = new URL(window.location);
        url.search = '';
        window.location = url.toString();
    }
}

function IsEmpty(str) {
    return (!str || str.length === 0);
}

function Sort(param, ascending) {
    let url = new URL(window.location);
    url.searchParams.set("parameters.Order.Property", param);
    url.searchParams.set("parameters.Order.Ascending", ascending);
    window.location = url.toString();
}

function SortByAscending(param) {
    Sort(param, true);
}

function SortByDescending(param) {
    Sort(param, false);
}