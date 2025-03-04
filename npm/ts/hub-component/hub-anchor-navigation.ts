const scrollToElement = (elementId: string): void => {
    const element: HTMLElement = document.getElementById(elementId);
    if (element instanceof HTMLElement) {
        element.scrollIntoView({
            behavior: "smooth",
            block: "start",
            inline: "nearest"
        });
    }
}

export default scrollToElement;