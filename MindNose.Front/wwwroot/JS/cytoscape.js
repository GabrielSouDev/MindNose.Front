class CytoscapeGraph {
    constructor(elementId, elements = [], selectedElements = [], layoutName = 'cose', dotNetRef = null) {
        this.elementId = elementId;
        this.elements = elements;
        this.selectedElements = selectedElements;
        this.layoutName = layoutName;
        this.layoutOptions = CytoscapeGraph.getLayoutOptions(layoutName);
        this.dotNetRef = dotNetRef;
        this.cy = null;
        this.init();
    }

    static getLayoutOptions(layoutName) {
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
    }

    static getStyle() {
        return [
            {
                selector: 'node',
                style: {
                    'shape': ele => ele.data('label') === 'Term' ? 'round-rectangle' : 'ellipse',
                    'label': ele => ele.data().extra.Title,
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'background-color': ele => {
                        const id = ele.id();
                        const label = ele.data('label');
                        const isSelected = ele.data('selected') === true;
                        if (isSelected) return '#27F5EE';
                        return label === 'Term' ? '#F16D6D' : '#F1BB6D';
                    },
                    'color': '#000',
                    'font-size': 14,
                    'padding': '10px',
                    'width': ele => {
                        const label = ele.data().extra.Title ?? '';
                        return Math.max(label.length * 7 + 20, 30);
                    }
                }
            },
            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': '#000',
                    'curve-style': 'bezier',
                    'target-arrow-color': '#000',
                    'target-arrow-shape': 'triangle'
                }
            }
        ];
    }

    init() {
        const container = document.getElementById(this.elementId);
        if (!container) {
            console.error(`Elemento com id "${this.elementId}" não encontrado.`);
            return;
        }

        this.cy = cytoscape({
            container: container,
            elements: this.elements,
            style: CytoscapeGraph.getStyle(),
            layout: this.layoutOptions
        });

        this.cy._tippies = [];
        this.registerEvents();
        this.applyTooltips();

        this.cy.on('layoutstop', () => {
            this.applyInitialSelection();
        });

        this.cy.layout(this.layoutOptions).run();
    }

    registerEvents() {
        this.cy.on('tap', 'node', evt => {
            const node = evt.target;
            const label = node.data('label');
            const nodeId = node.id();

            if (this.dotNetRef) {
                this.dotNetRef.invokeMethodAsync('OnNodeLeftClicked', nodeId);
            }

            const isSelected = node.data('selected') === true;
            const newSelected = !isSelected;
            node.data('selected', newSelected);

            const newColor = (label === 'Term')
                ? (newSelected ? '#27F5EE' : '#F16D6D')
                : (newSelected ? '#27F5EE' : '#F1BB6D');

            node.style('background-color', newColor);

            if (newSelected) {
                this.selectedElements.push(nodeId);
            } else {
                this.selectedElements = this.selectedElements.filter(id => id !== nodeId);
            }
        });

        this.cy.on('cxttap', 'node', evt => {
            const nodeId = evt.target.id();
            if (this.dotNetRef) {
                this.dotNetRef.invokeMethodAsync('OnNodeRightClicked', nodeId);
            }
        });
    }

    applyTooltips() {
        this.cy._tippies = [];

        this.cy.nodes().forEach(node => {
            const content = node.data().extra?.Summary ?? '(sem resumo)';
            const tip = tippy(this.cy.container(), {
                content,
                trigger: 'manual',
                placement: 'top',
                hideOnClick: false,
                arrow: true,
                theme: 'light-border',
            });

            this.cy._tippies.push(tip);

            node.on('mouseover', e => {
                const pos = e.target.renderedPosition();
                const rect = this.cy.container().getBoundingClientRect();
                const x = rect.left + pos.x;
                const y = rect.top + pos.y - 10;

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

    applyInitialSelection() {
        this.cy.nodes().forEach(node => {
            const nodeId = node.id();
            const isSelected = this.selectedElements.includes(nodeId);
            node.data('selected', isSelected);
            if (isSelected) {
                const label = node.data('label');
                const selectedColor = label === 'Term' ? '#27F5EE' : '#27F5EE';
                node.style('background-color', selectedColor);
            }
        });
    }

    addElement(element) {
        if (!element || typeof element !== 'object') {
            console.error('Elemento inválido para adicionar:', element);
            return;
        }
        this.cy.add(element);
        this.applyTooltips();
        this.applyInitialSelection();
        this.cy.layout(this.layoutOptions).run();
    }

    addElements(elements) {
        if (!Array.isArray(elements) || elements.length === 0) {
            console.error('Array de elementos inválido ou vazio:', elements);
            return;
        }
        this.cy.add(elements);
        this.applyTooltips();
        this.applyInitialSelection();
        this.cy.layout(this.layoutOptions).run();
    }

    relayout(layoutName) {
        this.layoutOptions = CytoscapeGraph.getLayoutOptions(layoutName);
        this.cy.layout(this.layoutOptions).run();
    }
}
