const box = document.getElementById("searchBox");
const list = document.getElementById("searchResults");

if (box && list) {
    box.addEventListener("input", function () {
        const text = box.value.toLowerCase();
        
        list.innerHTML = "";
        
        if (text.length < 2) {
            return;
        }
        
        const matches = searchIndex.filter(x => x.name.toLowerCase().includes(text)).slice(0, 100);
        
        for (const item of matches) {
            const li = document.createElement("li");

            li.className = "list-group-item";

            li.innerHTML = `
                <strong>${item.name}</strong>
                <br>
                <small class="text-muted">
                    ${item.path}
                </small>
            `;

            list.appendChild(li);
        }
    });
}