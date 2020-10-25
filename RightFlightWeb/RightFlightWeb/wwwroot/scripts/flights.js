
class Element {

    constructor(elementId) {
        this._htmlElement = document.getElementById(elementId);
    }

    hide() {
        this._htmlElement.classList.add('hidden');
    }

    unhide() {
        this._htmlElement.classList.remove('hidden');
    }

    clear() {
        this._htmlElement.innerHTML = '';
    }
}

class SelectionDisplay extends Element {

    constructor(elementId, clearSelection) {

        super(elementId);

        this.clearSelection = clearSelection;
    }

    setData(airport) {

        let airportElement = document.createElement('div');
        airportElement.classList.add('text-medium');

        airportElement.innerHTML = `${airport.name} (${airport.iataCode})`;
        this._htmlElement.appendChild(airportElement);

        let cityElement = document.createElement('div');
        cityElement.classList.add('text-small');

        cityElement.innerHTML = `${airport.city}, ${airport.country}`;
        this._htmlElement.appendChild(cityElement);

        let changeLink = document.createElement('div');
        changeLink.classList.add('change-link');
        changeLink.innerHTML = 'Change...';
        changeLink.addEventListener('click', this.clearSelection);
        this._htmlElement.appendChild(changeLink);
    }
}

class SuggestionBox extends Element {

    constructor(elementId) {
        super(elementId);
    }

    addSuggestion(suggestion, onItemSelected) {

        let listItem = document.createElement('li');
        listItem.classList.add('location-result');
        listItem.addEventListener('click', () => onItemSelected(suggestion))

        let airportElement = document.createElement('div');
        airportElement.classList.add('text-medium');

        airportElement.innerHTML = `${suggestion.name} (${suggestion.iataCode})`;
        listItem.appendChild(airportElement);

        let cityElement = document.createElement('div');
        cityElement.classList.add('text-small');

        cityElement.innerHTML = `${suggestion.city}, ${suggestion.country}`;
        listItem.appendChild(cityElement);

        this._htmlElement.appendChild(listItem);
    }
}

class SearchBox {

    constructor(args) {

        this.searchButton = document.getElementById(args.searchButtonId);
        this.searchInput = document.getElementById(args.searchInputId);
        this.codeInput = document.getElementById(args.codeInputId);

        this.suggestionBox = new SuggestionBox(args.suggestionBoxId);
        this.searchContainer = new Element(args.searchContainerId);
        this.selectionDisplay = new SelectionDisplay(args.selectionDisplayId, this.clearSelection);

        this.selectedEndpoint = null;

        this.searchInput.addEventListener('input', this.onSearchInput);
    }

    onSearchInput = e => {

        let searchText = this.searchInput.value.trim();

        if (searchText === null || searchText === '')
            return;

        if (searchText.length < 3) {
            this.clearSuggestions();
        }
        else {
            this.loadSuggestions();
        }
    };

    loadSuggestions() {

        let searchText = this.searchInput.value;

        let url = `https://localhost:5001/api/airport?query=${searchText}`;

        let request = new XMLHttpRequest();

        request.open('GET', url, true);

        request.onload = () => {

            if (request.status === 200) {
                this.populateSuggestions(JSON.parse(request.responseText));
            }
        };

        request.send();
    }

    populateSuggestions(suggestions) {

        if (suggestions.length === 0) {
            this.suggestionBox.hide();
        }
        else {
            this.suggestionBox.unhide();
        }

        this.suggestionBox.clear();

        for (let s of suggestions) {
            this.suggestionBox.addSuggestion(s, this.selectEndpoint);
        }
    }

    clearSuggestions() {
        this.suggestionBox.clear();
        this.suggestionBox.hide();
    }

    selectEndpoint = airport => {

        this.selectedEndpoint = airport;
        this.codeInput.value = airport.iataCode;

        this.selectionDisplay.setData(airport);

        this.selectionDisplay.unhide();
        this.searchContainer.hide();

        this.suggestionBox.clear();
        this.suggestionBox.hide();

        this.searchInput.value = '';
    };

    clearSelection = () => {

        this.selectedEndpoint = null;
        this.codeInput.value = '';

        this.selectionDisplay.clear();
        this.selectionDisplay.hide();

        this.searchContainer.unhide();
    };
}

let originSearchBoxArgs = {

    searchButtonId: 'origin-search-button',
    suggestionBoxId: 'origin-results',
    searchInputId: 'origin-search-input',
    selectionDisplayId: 'selected-origin',
    codeInputId: 'selected-origin-code',
    searchContainerId: 'origin-search'
};

let destinationSearchBoxArgs = {

    searchButtonId: 'destination-search-button',
    suggestionBoxId: 'destination-results',
    searchInputId: 'destination-search-input',
    selectionDisplayId: 'selected-destination',
    codeInputId: 'selected-destination-code',
    searchContainerId: 'destination-search'
};

let originSearchBox = new SearchBox(originSearchBoxArgs);
let destinationSearchBox = new SearchBox(destinationSearchBoxArgs);
