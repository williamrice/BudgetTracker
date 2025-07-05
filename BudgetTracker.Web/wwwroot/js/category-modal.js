function setupCategoryModal(selectId, triggerBtnId) {
    const modal = document.getElementById('categoryModal');
    const addBtn = document.getElementById(triggerBtnId);
    const closeBtn = document.getElementById('closeCategoryModal');
    const form = document.getElementById('categoryForm');
    const errorDiv = document.getElementById('categoryError');
    const catSelect = document.getElementById(selectId);
    const colorPicker = document.getElementById('catColor');
    const colorText = document.getElementById('catColorText');

    // Sync color picker with text input
    colorPicker.addEventListener('change', function() {
        colorText.value = this.value;
    });
    
    colorText.addEventListener('change', function() {
        if (this.value.match(/^#[0-9A-F]{6}$/i)) {
            colorPicker.value = this.value;
        }
    });

    // Open dialog
    addBtn.addEventListener('click', function() {
        modal.showModal();
        errorDiv.style.display = 'none';
        form.reset();
        // Reset color to default
        colorPicker.value = '#1e3c72';
        colorText.value = '#1e3c72';
        // Focus the first input
        document.getElementById('catName').focus();
    });
    
    // Close dialog
    closeBtn.addEventListener('click', function() {
        modal.close();
    });
    
    // Close dialog when clicking outside (on backdrop)
    modal.addEventListener('click', function(event) {
        if (event.target === modal) {
            modal.close();
        }
    });

    // Handle escape key
    modal.addEventListener('cancel', function(event) {
        // Allow default behavior (closes dialog)
    });
    
    // Handle form submission
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        errorDiv.style.display = 'none';
        
        const data = {
            Name: form.catName.value.trim(),
            Description: form.catDesc.value.trim(),
            Color: colorText.value || '#1e3c72'
        };
        
        if (!data.Name) {
            errorDiv.textContent = 'Name is required.';
            errorDiv.style.display = 'block';
            return;
        }
        
        try {
            const resp = await fetch('/Category/CreateQuick', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            
            if (!resp.ok) {
                const err = await resp.text();
                throw new Error(err);
            }
            
            const category = await resp.json();
            const option = document.createElement('option');
            option.value = category.id || category.Id;
            option.text = category.name || category.Name;
            option.selected = true;
            catSelect.appendChild(option);
            
            modal.close();
        } catch (err) {
            errorDiv.textContent = err.message || 'Failed to add category.';
            errorDiv.style.display = 'block';
        }
    });
}
