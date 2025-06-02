// document.addEventListener("DOMContentLoaded", function () {
//     const blocks = document.querySelectorAll('.content-block');

//     const observer = new IntersectionObserver((entries, observer) => {
//       entries.forEach(entry => {
//         if (entry.isIntersecting) {
//           entry.target.classList.add('visible'); // Show block when in view
//         } else {
//           entry.target.classList.remove('visible'); // Hide block when out of view
//         }
//       });
//     }, { threshold: 0.7 }); // Adjust visibility threshold to 70%

//     blocks.forEach(block => {
//       observer.observe(block);
//     });

//     const sq_blocks = document.querySelectorAll('.sq-block');

//     sq_blocks.forEach(sq_block => {
//       sq_block.addEventListener('click', function() {
//         const url = sq_block.getAttribute('data-url'); // Get URL from data attribute
//         if (url) {
//           window.location.href = url // Open the URL in a new tab
//         }
//       });
//     });
//   });


