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


    const wordsCountRes = await fetch("/crawl/wordCount/getAll");
    const wordsCount = await wordsCountRes.json();
    console.log("Words Count:", wordsCount);


    const tbody = table.getElementsByTagName("tbody")[0]
    tbody.innerHTML = "";

    for (const wordCount of wordsCount) {
        row = document.createElement("tr");
        row.innerHTML = `
            <td>${wordCount.id}</td>
            <td>${wordCount.crawlId}</td>
            <td>${wordCount.crawlUrlId}</td>
            <td>${wordCount.count}</td>
            <td>${wordCount.word}</td>
        `;
        tbody.appendChild(row);
    }


});

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const startUrls = Array.from(document.querySelectorAll(".urlInput"))
            .map(i => i.value.trim())
            .filter(v => v.length > 0)
            .join(",");

        const keywords = Array.from(document.querySelectorAll(".keywordInput"))
            .map(i => i.value.trim())
            .filter(v => v.length > 0)
            .join(",");

        const data = {
            startUrls,
            keywords,
            maxDepth: parseInt(document.getElementById("maxDepth").value),
            maxChildLinks: parseInt(document.getElementById("maxChildLinks").value),
            crawlPeriod: parseInt(document.getElementById("delay").value)
        };

        await fetch("/crawl/config", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });
    });


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
    });
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
