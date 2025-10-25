window.cytoscapeInterop = {
    instances: {},
    dotNetRef: null,

    setDotNetReference: function (ref) {
        this.dotNetRef = ref;
    },

    init: function (elementId, elements, selectedElements, layoutName = 'cose') {
        const container = document.getElementById(elementId);
        if (!container) {
            console.error(`Elemento com id "${elementId}" não encontrado.`);
            return;
        }

        if (this.instances[elementId]) {
            try {
                this.instances[elementId].cy.destroy();
            } catch (e) {
                console.warn("Falha ao destruir instância antiga:", e);
            }
            delete this.instances[elementId];
        }

        const graph = new CytoscapeGraph(
            elementId,
            elements,
            selectedElements,
            layoutName,
            this.dotNetRef
        );

        this.instances[elementId] = graph;
    },

    add: function (elementId, element) {
        const graph = this.instances[elementId];
        if (graph) {
            graph.addElement(element);
        }
    },

    addrange: function (elementId, elements) {
        const graph = this.instances[elementId];
        if (graph) {
            graph.addElements(elements);
        }
    },

    relayout: function (elementId, layoutName) {
        const graph = this.instances[elementId];
        if (graph) {
            graph.relayout(layoutName);
        }
    }
};