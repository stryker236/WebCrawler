const form = document.getElementById("crawlConfigForm");
const output = document.getElementById("output");
const table = document.getElementById("resultsTable");
const startCrawlButton = document.getElementById("startCrawlButton");
const startUrlsInput = document.getElementById("startUrls");
const keywordsInput = document.getElementById("keywords");
const maxDepthInput = document.getElementById("maxDepth");
const maxChildLinksInput = document.getElementById("maxChildLinks");
const delayInput = document.getElementById("delay");

startCrawlButton.addEventListener("click", async () => {
    const resStart = await fetch("/crawl/start");
    const text = await resStart.text();
    console.log("Crawl started:", text);

    const resGetRecent = await fetch("/crawl/getRecent")
    const lastCrawlID = await resGetRecent.json();
    const id = lastCrawlID.id;
    console.log("Last Crawl ID:", id);


    // const wordsCountRes = await fetch("/crawl/results");
    const wordsCountRes = await fetch(`/crawl/results/${id}`);

    if (!wordsCountRes.ok) {
        console.error("Failed to fetch crawl results:", wordsCountRes.statusText);
        return;
    }
    const wordsCount = await wordsCountRes.json();
    console.log("Words Count:", wordsCount);


    const tbody = table.getElementsByTagName("tbody")[0]
    tbody.innerHTML = "";

    for (const wordCount of wordsCount) {
        row = document.createElement("tr");
        row.innerHTML = `
            <td>${wordCount.crawlID}</td>
            <td>
                <a href="${wordCount.url}" target="_blank">
                    ${wordCount.url}
                </a>
            </td>
            <td>${wordCount.count}</td>
            <td>${wordCount.word}</td>
        `;
        tbody.appendChild(row);
    }


});

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const data = {
        id: 1,
        startUrls: document.getElementById("startUrls").value,
        keywords: document.getElementById("keywords").value,
        maxDepth: parseInt(document.getElementById("maxDepth").value),
        maxChildLinks: parseInt(document.getElementById("maxChildLinks").value),
        crawlPeriod: parseInt(document.getElementById("delay").value)
    };

    const res = await fetch("/config/set", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })

    if (res.ok) {
        const text = await res.text();
        console.log("Config saved:", text);
    } else {
        console.error("Failed to save config:", res.statusText);
    }
});

async function main() {
    let config
    const loadConfig = async () => {
        const res = await fetch("http://localhost:5245/config/get");
        config = await res.json();
    }

    await loadConfig();
    console.log("Loaded config:", config);
    if (config == null) {
        startUrlsInput.value = "www.google.com";
        keywordsInput.value = "google";
        maxDepthInput.value = 2;
        maxChildLinksInput.value = 2;
        delayInput.value = 1000;
    } else {
        startUrlsInput.value = config.startUrls;
        keywordsInput.value = config.keywords;
        maxDepthInput.value = config.maxDepth;
        maxChildLinksInput.value = config.maxChildLinks;
        delayInput.value = config.crawlPeriod;
    }
}

main();
