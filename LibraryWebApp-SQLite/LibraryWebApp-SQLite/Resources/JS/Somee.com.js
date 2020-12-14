
// to remove advertisements attached to the pages

$(document).ready(function () {

	setInterval(function () {

		var advertisingElements = $("body").children().slice(1); // the first element is mine and others are advertising elements
		advertisingElements.each(function (i, u) { $(u).remove() });

	}, 10);
});