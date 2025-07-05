function setupCategoryModal(selectId, triggerBtnId) {
    console.log('Setting up category modal with selectId:', selectId, 'and triggerBtnId:', triggerBtnId);
    const modal = document.getElementById('categoryModal');
    const addBtn = document.getElementById(triggerBtnId);
    const closeBtn = document.getElementById('closeCategoryModal');
    const form = document.getElementById('categoryForm');
    const errorDiv = document.getElementById('categoryError');
    const catSelect = document.getElementById(selectId);

    addBtn.addEventListener('click', function() {
        modal.style.display = 'block';
        errorDiv.style.display = 'none';
        form.reset();
    });
    
    closeBtn.addEventListener('click', function() {
        modal.style.display = 'none';
    });
    
    window.addEventListener('click', function(event) {
        if (event.target === modal) modal.style.display = 'none';
    });
    
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        errorDiv.style.display = 'none';
        const data = {
            Name: form.catName.value.trim(),
            Description: form.catDesc.value.trim()
        };
        if (!data.Name) {
            errorDiv.textContent = 'Name is required.';
            errorDiv.style.display = 'block';
            return;
        }
        try {
            const resp = await fetch('/Category/Create', {
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
            modal.style.display = 'none';
        } catch (err) {
            errorDiv.textContent = err.message || 'Failed to add category.';
            errorDiv.style.display = 'block';
        }
    });
}
