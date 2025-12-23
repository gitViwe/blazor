const scrollToElement = (elementId: string): void => {
    const element: HTMLElement | null = document.getElementById(elementId);
    if (element instanceof HTMLElement) {
        element.scrollIntoView({
            behavior: "smooth",
            block: "start",
            inline: "nearest"
        });
    }
}

export default scrollToElement;