// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const filterBtn = document.getElementById("FiltersDiv");

function showFilters()
{
    if (filterBtn.style.display === "block") {
        filterBtn.style.display = "none";
    }
    else
    {
        filterBtn.style.display = "block";
    }
}

// animatie voor logo als pagina laad
document.addEventListener("DOMContentLoaded", function () {
    const logo = document.getElementById('logoDiv');
    logo.classList.add('fade-in');
});

// film toevoegen aan watchList

