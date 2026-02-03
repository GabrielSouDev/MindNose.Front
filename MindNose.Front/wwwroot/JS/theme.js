window.setTheme = function (theme) {
    const normalized = (theme || "").toLowerCase();

    let link = document.getElementById("theme-css");

    if (!link) {
        link = document.createElement("link");
        link.rel = "stylesheet";
        link.id = "theme-css";
        document.head.appendChild(link);
    }

    const newHref = normalized.includes("dark")
        ? "/css/dark-theme.css"
        : "/css/light-theme.css";

    if (link.href !== location.origin + newHref) {
        link.href = newHref;
    }
};