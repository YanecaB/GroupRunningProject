﻿document.addEventListener('DOMContentLoaded', function () {
    const becomeFriendsButton = document.getElementById('becomeFriend');

    if (becomeFriendsButton) {
        becomeFriendsButton.addEventListener('click', function () {
            const query = becomeFriendsButton.getAttribute('data-username');

            fetch(`https://localhost:7018/api/SearchApi/SendFriendRequest?username=${encodeURIComponent(query)}`, {
                method: 'POST',
                credentials: 'include'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to send friend request.");
                    }

                    alert('The friend request sent successfully!');
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while sending the friend request.');
                });
        });
    } else {
        console.error('Button with id "becomeFriend" not found!');
    }
});
