// Dashboard functionality
document.addEventListener('DOMContentLoaded', function() {
    // Sidebar toggle functionality
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');

    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function() {
            // For mobile, use different classes
            if (window.innerWidth <= 991.98) {
                sidebar.classList.toggle('show');
                // Add backdrop for mobile
                if (sidebar.classList.contains('show')) {
                    addBackdrop();
                } else {
                    removeBackdrop();
                }
            } else {
                sidebar.classList.toggle('collapsed');
                content.classList.toggle('expanded');
            }
        });
    }

    // Auto-manage sidebar based on screen size
    function checkScreenSize() {
        removeBackdrop(); // Remove any existing backdrop
        
        if (window.innerWidth <= 991.98) {
            // Mobile: hide sidebar by default
            sidebar.classList.remove('collapsed', 'show');
            content.classList.remove('expanded');
        } else {
            // Desktop: show sidebar normally
            sidebar.classList.remove('show', 'collapsed');
            content.classList.remove('expanded');
        }
    }

    // Add backdrop for mobile sidebar
    function addBackdrop() {
        let backdrop = document.querySelector('.sidebar-backdrop');
        if (!backdrop) {
            backdrop = document.createElement('div');
            backdrop.className = 'sidebar-backdrop';
            backdrop.style.cssText = `
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background: rgba(0, 0, 0, 0.5);
                z-index: 1039;
            `;
            backdrop.addEventListener('click', function() {
                sidebar.classList.remove('show');
                removeBackdrop();
            });
            document.body.appendChild(backdrop);
        }
    }

    // Remove backdrop
    function removeBackdrop() {
        const backdrop = document.querySelector('.sidebar-backdrop');
        if (backdrop) {
            backdrop.remove();
        }
    }

    // Check screen size on load and resize
    checkScreenSize();
    window.addEventListener('resize', checkScreenSize);

    // Close mobile sidebar when clicking on nav links
    const sidebarLinks = sidebar.querySelectorAll('a');
    sidebarLinks.forEach(link => {
        link.addEventListener('click', function() {
            if (window.innerWidth <= 991.98 && sidebar.classList.contains('show')) {
                sidebar.classList.remove('show');
                removeBackdrop();
            }
        });
    });

    // Add fade-in animation to cards
    const cards = document.querySelectorAll('.dashboard-card, .stat-card');
    cards.forEach((card, index) => {
        card.style.animationDelay = `${index * 0.1}s`;
        card.classList.add('fade-in');
    });

    // Format currency values
    const currencyElements = document.querySelectorAll('[data-currency]');
    currencyElements.forEach(element => {
        const value = parseFloat(element.textContent);
        element.textContent = formatCurrency(value);
    });

    // Confirm delete actions
    const deleteButtons = document.querySelectorAll('[data-confirm-delete]');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            const message = this.getAttribute('data-confirm-delete') || 'Are you sure you want to delete this item?';
            if (!confirm(message)) {
                e.preventDefault();
            }
        });
    });
});

// Helper function to format currency
function formatCurrency(value) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(value);
}

// Function to show notifications (mobile-friendly)
function showNotification(message, type = 'success') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    
    // Mobile-responsive positioning
    const isMobile = window.innerWidth <= 575.98;
    notification.style.cssText = isMobile 
        ? 'top: 10px; left: 10px; right: 10px; z-index: 9999;'
        : 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto-remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

// AJAX form submission helper
function submitFormAjax(form, successCallback) {
    const formData = new FormData(form);
    
    fetch(form.action, {
        method: form.method,
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showNotification(data.message || 'Operation completed successfully!');
            if (successCallback) successCallback(data);
        } else {
            showNotification(data.message || 'An error occurred.', 'danger');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        showNotification('An unexpected error occurred.', 'danger');
    });
}
