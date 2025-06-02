// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const viewportWidth = window.innerWidth;
const viewportHeight = window.innerHeight;

function isElementInViewport(el) {
  var rect = el.getBoundingClientRect();
  return (rect.top >= 0 && rect.bottom <= $(window).height());
}

function onScroll() {
  var elements = document.querySelectorAll(".content-block");
  for (var i = 0; i < elements.length; i++) {
    var element = elements[i];
    if (isElementInViewport(element)) {
      element.style.opacity = 1;
      element.style.transition = "5s ease";
    } else {
      element.style.opacity = 0;
    }
  }
}

window.addEventListener("scroll", onScroll)

const sq_blocks = document.querySelectorAll(".sq-block");

sq_blocks.forEach((sq_block) => {
  sq_block.addEventListener("click", function () {
    const url = sq_block.getAttribute("data-url"); // Get URL from data attribute
    if (url) {
      window.location.href = url; // Open the URL in a new tab
    }
  });
});


const bgImgDiv = document.querySelectorAll(".bg-ai-img")

bgImgDiv.forEach(function(div){

  const divId = div.id;

  const imageUrl = `../uploads/${divId}`

  div.style.backgroundImage = `url('${imageUrl}')`
  div.style.backgroundSize =  "cover"
})