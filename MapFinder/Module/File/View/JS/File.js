file = {
    parent: function () {
        return document.getElementsByClassName("fileWrapper")[0];//TD:  Если несколько
    },
    newElement: function () {
        var div = document.createElement('div');
        div.className = "file";
        div.innerHTML = "<input type='file' name='files' />";
        return div;
    },
    add: function () {
        var div = this.newElement();
        var p = this.parent();
        p.appendChild(newLi);
    }

}