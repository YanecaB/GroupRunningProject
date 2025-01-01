document.addEventListener('DOMContentLoaded', function () {
    const becomeFriendsButton = document.getElementById('becomeFriend');
    const deleteButton = document.getElementById('deleteFriendRequest');

    function hideCurrentButton(showSendButton) {
        becomeFriendsButton.style.display = showSendButton ? 'block' : 'none';
        deleteButton.style.display = showSendButton ? 'none' : 'block';
    }

    if (becomeFriendsButton) {
        becomeFriendsButton.addEventListener('click', function () {
            const query = becomeFriendsButton.getAttribute('data-username');

            fetch(`https://localhost:7015/api/FriendRequestApi/SendFriendRequest?username=${encodeURIComponent(query)}`, {
                method: 'GET',
                credentials: 'include'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to send friend request.");
                    }

                    hideCurrentButton(false);

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

    if (deleteButton) {
        deleteButton.addEventListener('click', function () {
            const query = deleteButton.getAttribute('data-username');

            fetch(`https://localhost:7015/api/FriendRequestApi/DeleteFriendRequest?username=${encodeURIComponent(query)}`, {
                method: 'GET',
                credentials: 'include'
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to delete friend request.");
                    }

                    hideCurrentButton(true);

                    alert('The friend request deleted successfully!');
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while deleting the friend request.');
                });
        });
    } else {
        console.error('Button with id "deleteFriendRequest" not found!');
    }
});