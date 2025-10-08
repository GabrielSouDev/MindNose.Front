window.cytoscapeInterop = {
    instances: {},

    getLayoutOptions: function (layoutName) {
        return {
            name: layoutName,
            idealEdgeLength: 100,
            nodeOverlap: 10,
            refresh: 15,
            fit: true,
            padding: 30,
            randomize: true,
            animate: true,
            animationDuration: 400
        };
    },

    init: function (elementId, elements, layoutName = 'cose') {
        const container = document.getElementById(elementId);
        if (!container) {
            console.error(`Elemento com id "${elementId}" não encontrado.`);
            return;
        }

        if (this.instances[elementId]) {
            try {
                this.instances[elementId].cy.destroy();
            } catch (e) {
                console.warn("Falha ao destruir instância antiga de Cytoscape:", e);
            }
            delete this.instances[elementId];
        }

        const layoutOptions = this.getLayoutOptions(layoutName);

        const cy = cytoscape({
            container: container,

            elements: elements ?? [],

            style: [
                {
                    selector: 'node',
                    style: {
                        'shape': function (ele) {
                            const label = ele.data('label');
                            if (label === 'Term') {
                                return 'round-rectangle'
                            } else {
                                return 'ellipse'
                            }
                        },
                        'label': function (ele) {
                            const data = ele.data();
                            return data.extra.Title;
                        },
                        'text-valign': 'center',
                        'text-halign': 'center',
                        'background-color': function (ele) {
                            const label = ele.data('label');
                            if (label === 'Term') {
                                return '#F16D6D'
                            } else {
                                return '#F1BB6D'
                            }

                        },
                        'color': '#000',
                        'font-size': 14,
                        'padding': '10px',
                        'width': function (ele) {
                            const data = ele.data ? ele.data() : {};
                            const label = data.extra.Title ?? '';
                            const padding = 20;
                            return Math.max(label.length * 7 + padding, 30);
                        }
                    }
                },

                {
                    selector: 'edge',
                    style: {
                        'width': 2,
                        'line-color': '#000',
                        "curve-style": "bezier",
                        'target-arrow-color': '#000',
                        'target-arrow-shape': 'triangle'
                    }
                }
            ],
            layout: layoutOptions
        });

        this.instances[elementId] = { cy, layoutOptions };

        cy.layout(layoutOptions).run();
    },

    add: function (elementId, element) {
        const instance = this.instances[elementId];
        if (!instance) {
            console.error(`Instância do Cytoscape com id "${elementId}" não encontrada.`);
            return;
        }
        const { cy, layoutOptions } = instance;

        if (!element || typeof element !== 'object') {
            console.error('Elemento inválido para adicionar:', element);
            return;
        }
        cy.add(element);

        cy.layout(layoutOptions).run();
    },

    addrange: function (elementId, elements) {
        const instance = this.instances[elementId];
        if (!instance) {
            console.error(`Instância do Cytoscape com id "${elementId}" não encontrada.`);
            return;
        }
        const { cy, layoutOptions } = instance;

        if (!Array.isArray(elements) || elements.length === 0) {
            console.error('Array de elementos inválido ou vazio:', elements);
            return;
        }
        cy.add(elements);

        cy.layout(layoutOptions).run();
    },

    relayout: function (elementId, layoutName) {
        const instance = this.instances[elementId];
        if (!instance) {
            console.error(`Instância do Cytoscape "${elementId}" não encontrada.`);
            return;
        }

        const { cy } = instance;
        const newLayout = this.getLayoutOptions(layoutName);

        this.instances[elementId] = { cy, layoutOptions: newLayout };
        cy.layout(newLayout).run();
    }
};