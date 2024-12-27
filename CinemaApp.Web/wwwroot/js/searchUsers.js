document.addEventListener('DOMContentLoaded', function () {
    const searchButton = document.getElementById('searchButton');
    const searchModal = document.getElementById('searchModal');
    const closeSearch = document.getElementById('closeSearch');
    const searchContent = document.getElementById('searchContent');
    const searchInput = document.getElementById('searchInput');

    searchButton.addEventListener('click', function (event) {
        event.preventDefault();
        searchModal.style.display = 'block'; 
    });

    closeSearch.addEventListener('click', function () {
        searchModal.style.display = 'none'; 
    });

    window.addEventListener('click', function (event) {
        if (event.target === searchModal) {
            searchModal.style.display = 'none';
        }
    });

    searchInput.addEventListener('input', function () {
        const query = searchInput.value;

        if (query.length > 0) {
            fetch(`https://localhost:7018/api/SearchApi/SearchUsers?username=${encodeURIComponent(query)}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to load search results.");
                    }
                    return response.json();
                })
                .then(data => {
                    const resultsContainer = document.getElementById('searchResults');
                    resultsContainer.innerHTML = ''; 

                    if (data.length > 0) {
                        data.forEach(user => {
                            const resultItem = document.createElement('div');
                            resultItem.className = 'search-result-item';

                            const profilePic = document.createElement('img');
                            profilePic.src = user.profilePicturePath || '/default-avatar.jpg';
                            profilePic.alt = 'Profile Picture';
                            profilePic.style.width = '30px';
                            profilePic.style.height = '30px';
                            profilePic.style.borderRadius = '50%';
                            profilePic.style.objectFit = 'cover';
                            profilePic.style.border = '0.5px solid #ddd';
                            profilePic.style.marginRight = '3%';
                            
                            const usernameElement = document.createElement('strong');
                            usernameElement.textContent = user.username;                           
                            
                            resultItem.appendChild(profilePic);
                            resultItem.appendChild(usernameElement);                            

                            resultsContainer.appendChild(resultItem);
                        });
                    } else {
                        resultsContainer.innerHTML = '<p>No users found.</p>';
                    }
                })
                .catch(error => {
                    console.error("Error loading search results:", error);
                });
        }
    });
});
