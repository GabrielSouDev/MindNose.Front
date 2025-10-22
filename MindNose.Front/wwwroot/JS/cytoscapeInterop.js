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
        cy._tippies = [];
        this.instances[elementId] = { cy, layoutOptions };

        this.applyTooltips(cy);
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
        this.applyTooltips(cy);

        cy.on('tap', 'node', function (evt) {
            const node = evt.target;
            const label = node.data('label');
            const nodeId = evt.target.id();
            DotNet.invokeMethodAsync('MindNose.Front', 'OnNodeLeftClicked', nodeId);

            const selected = node.data('selected') || false;
            node.data('selected', !selected);

            if (label === 'Term') {
                node.style('background-color', selected ? '#F16D6D' : '#27F5EE');
            } else {
                node.style('background-color', selected ? '#F1BB6D' : '#27F5EE');
            }
        });

        cy.on('cxttap', 'node', function (evt) {
            const nodeId = evt.target.id();
            DotNet.invokeMethodAsync('MindNose.Front', 'OnNodeRightClicked', nodeId);
        });

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
        this.applyTooltips(cy);

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
    },
    applyTooltips: function (cy) {
        // Inicializa o array de tippies se ainda não existir
        cy._tippies = cy._tippies || [];

        cy.nodes().forEach(function (node) {
            const content = node.data().extra?.Summary ?? '';

            // Cria o tooltip associado ao container do Cytoscape
            const tip = tippy(cy.container(), {
                content: content || '(sem resumo)',
                trigger: 'manual',
                placement: 'top',
                hideOnClick: false,
                arrow: true,
                theme: 'light-border',
            });

            cy._tippies.push(tip);

            // Eventos de hover
            node.on('mouseover', (e) => {
                const pos = e.target.renderedPosition();
                const rect = cy.container().getBoundingClientRect();

                // Converte posição do Cytoscape (canvas) para coordenadas de tela
                const x = rect.left + pos.x;
                const y = rect.top + pos.y - 10; // leve deslocamento pra cima

                tip.setProps({
                    getReferenceClientRect: () => ({
                        width: 0,
                        height: 0,
                        top: y,
                        bottom: y,
                        left: x,
                        right: x,
                    }),
                });

                tip.show();
            });

            node.on('mouseout', () => tip.hide());
        });
    }
};