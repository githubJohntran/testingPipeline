function getElementFromRoot(rootNode, cssSelectorChild, subFrameId) {
    var element;
    if (!subFrameId) {
        element = rootNode.querySelector(cssSelectorChild);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        element = rootNode.querySelector(cssSelectorChild);
    }
    return element;
}

function getElementsFromRoot(rootNode, cssSelectorChild, subFrameId) {
    var nodeList;
    if (!subFrameId) {
        nodeList = rootNode.querySelectorAll(cssSelectorChild);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        nodeList = rootNode.querySelectorAll(cssSelectorChild);
    }

    return nodeList;
}

function getElementText(cssSelectorChild, subFrameId) {
    var element;
    if (!subFrameId) {
        element = document.querySelector(cssSelectorChild);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        element = subFrame.contentDocument.querySelector(cssSelectorChild);
    }
    return element.innerText;
}

function getElementsText(cssSelector, subFrameId) {
    let list = [];
    let items = [];
    if (!subFrameId) {
        items = document.querySelectorAll(cssSelector);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        items = subFrame.contentDocument.querySelectorAll(cssSelector);
    }

    for (let i = 0; i < items.length; i++) {
        list.push(items[i].innerText);
    }

    return list;
}

function getElementAttributeValue(cssSelectorChild, subFrameId) {
    var element;
    if (!subFrameId) {
        element = document.querySelector(cssSelectorChild);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        element = subFrame.contentDocument.querySelector(cssSelectorChild);
    }
    return element.getAttribute(attribute);
}

function getElementsAttributeValue(cssSelector, attribute, subFrameId) {
    let list = [];
    let items = [];
    if (!subFrameId) {
        items = document.querySelectorAll(cssSelector);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        items = subFrame.contentDocument.querySelectorAll(cssSelector);
    }

    for (let i = 0; i < items.length; i++) {
        list.push(items[i].getAttribute(attribute));
    }

    return list;
}

function isElementInvisible(cssSelector, subFrameId) {
    var element;
    if (!subFrameId) {
        element = document.querySelector(cssSelector);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        element = subFrame.contentDocument.querySelector(cssSelector);
    }
    return (element.offsetParent === null);
}

function clickOnElement(cssSelector, subFrameId) {
    var element;
    if (!subFrameId) {
        element = document.querySelector(cssSelector);
    }
    else {
        var subFrame = document.getElementById(subFrameId);
        subFrame.contentWindow.focus();
        element = subFrame.contentDocument.querySelector(cssSelector);
    }
    element.scrollIntoView();
    element.click();
}

function getGridTable(rootNode) {
    var result = [];
    var container = rootNode;
    if (rootNode.className.indexOf('w2ui-grid-records') == -1) {
        container = rootNode.querySelector('.w2ui-grid-records');
    }
    var oTable = container.querySelector('table');

    //gets rows of table
    var rowLength = oTable.rows.length;

    //loops through rows 
    var rowIdx = 0;
    for (var i = 0; i < rowLength; i++) {
        var row = oTable.rows[i];
        if (row.getAttribute("recid")) {
            result[rowIdx] = [];
            //gets cells of current row
            var oCells = row.cells;

            //gets amount of cells of current row
            var cellLength = oCells.length;

            //loops through each cell in current row
            for (var j = 0; j < cellLength; j++) {
                var cell = oCells[j];
                if (cell.className.indexOf('w2ui-grid-data-last') == -1) {
                    var cellVal = null;
                    /* get your cell info here */
                    var iconStatus = null;
                    if (iconStatus = cell.querySelector('div.failure-grid-icon-status')) {
                        if (iconStatus.getAttribute('style').indexOf('status-warning.png') != -1 ||
						iconStatus.getAttribute('style').indexOf('status-error.png') != -1) {
                            cellVal = 'true';
                        }
                        else if (iconStatus.getAttribute('style').indexOf('status-ok.png') != -1) {
                            cellVal = 'false';
                        }
                    }
                    else {
                        cellVal = cell.innerText;
                        cellVal = cellVal.replace('%', '');
                    }
                    cellVal = cellVal.replace('\n', '');
                    result[rowIdx][j] = cellVal;
                }
            }
            rowIdx++;
        }
    }
    return result;
}

function simulateDragDrop(sourceNode, destinationNode) {
    var EVENT_TYPES = {
        DRAG_END: 'dragend',
        DRAG_START: 'dragstart',
        DROP: 'drop'
    }

    function createCustomEvent(type) {
        var event = document.createEvent("CustomEvent");
        event.initCustomEvent(type, true, true, null);
        event.dataTransfer = {
            data: {
            },
            setData: function (type, val) {
                this.data[type] = val;
            },
            getData: function (type) {
                return this.data[type];
            }
        }
        return event;
    }

    function dispatchEvent(node, type, event) {
        if (node.dispatchEvent) {
            return node.dispatchEvent(event);
        }
        if (node.fireEvent) {
            return node.fireEvent("on" + type, event);
        }
    }

    var event = createCustomEvent(EVENT_TYPES.DRAG_START);
    dispatchEvent(sourceNode, EVENT_TYPES.DRAG_START, event);

    var dropEvent = createCustomEvent(EVENT_TYPES.DROP);
    dropEvent.dataTransfer = event.dataTransfer;
    dispatchEvent(destinationNode, EVENT_TYPES.DROP, dropEvent);

    var dragEndEvent = createCustomEvent(EVENT_TYPES.DRAG_END);
    dragEndEvent.dataTransfer = event.dataTransfer;
    dispatchEvent(sourceNode, EVENT_TYPES.DRAG_END, dragEndEvent);
}