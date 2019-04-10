(function(window, undefined) {
    function ApiService(apiBaseUrl) {
        let self = this;
        
        self.apiBaseUrl = apiBaseUrl;
        
        self.listItems = function() {
            return $.ajax({
                url: self.apiBaseUrl() + "api/todo",
                type: 'GET'
            });
        };
        self.createItem = function(label, copyFromId) {
            let command = {
                "label": label,
                "copy_from": copyFromId
            };
            return $.ajax({
                url: self.apiBaseUrl() + "api/todo",
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify(command)
            });
        };
        self.updateItem = function(id, label) {
            let command = {
                "label": label
            };
            return $.ajax({
                url: self.apiBaseUrl() + "api/todo/" + id,
                type: 'PUT',
                contentType: "application/json",
                data: JSON.stringify(command)
            });
        };
        self.removeItem = function(id) {
            return $.ajax({
                url: self.apiBaseUrl() + "api/todo/" + id,
                type: 'DELETE'
            });
        };
    }
    
    function Item(item, $parent) {
        let self = this;
        
        self.id = item.id;
        self.label = ko.observable(item.label);
        self.created = ko.observable(item.created);
        self.updated = ko.observable(item.updated);
        
        self.reload = function(item) {
            self.label(item.label);
            self.created(item.created);
            self.updated(item.updated);
        };
    }
    
    function ViewModel() {
        let self = this;
        
        self.apiBaseUrl = ko.observable("http://local.todo-api.com/");
        self.newItemLabel = ko.observable("");
        self.items = ko.observableArray([]);
        
        let api = new ApiService(self.apiBaseUrl);
        
        self.initialize = function () {
            self.items.removeAll();
            return api.listItems()
                .done(function (items) { items.forEach(x => self.items.push(new Item(x, self))); })
                .fail(function () { alert("Faild to load items"); });
        };
        
        self.add = function () {
            if (self.newItemLabel() !== "") {
                return api.createItem(self.newItemLabel(), null)
                    .done(function (item) {
                        self.items.push(new Item(item, self));
                        self.newItemLabel("");
                    })
                    .fail(function () { alert("Failed to create new item"); });
            }
        };
        self.update = function (item) {
            return api.updateItem(item.id, item.label())
                .done(function (result) { item.reload(result); })
                .fail(function () { alert("Failed to update item"); });
        };
        self.copy = function (item) {
            return api.createItem(null, item.id)
                .done(function (result) { self.items.push(new Item(result, self)); })
                .fail(function () { alert("Failed to copy item"); });
        };
        self.remove = function (item) {
            return api.removeItem(item.id, item.label(), null)
                .done(function () { self.items.remove(item); })
                .fail(function () { alert("Failed to delete item"); });
        };
    }
    
    let vm = new ViewModel();
    vm.initialize().done(function () { ko.applyBindings(vm); });
})(window);
