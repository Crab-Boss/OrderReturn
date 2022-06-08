// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var arrLang = {
	"en": {
		"title1": "Create return label for",
		"title2": "Your return delivery to FDS GmbH",
		"pleaseEnter": "Please enter your sender details in the following fields and click \"Next\".",
		"orderNumber": "Order Number",
		"name": "First and last name",
		"country": "Country",
		"address": "Street Address",
		"streetName": "Street Name",
		"houseNumber": "House Number",
		"telephoneNumber": "Phone Number",
		"zip": "ZIP",
		"city": "City",
		"email": "E-Mail",
		"reason": "Return reason",
		"next": "Next",
		"labelBeenCreated": "The return label has been created",
		"returnLabel": "Return label",
		"printLabel": "You can now download the return label as a PDF file and print it out. Please stick the label on the largest side of your parcel afterwards.",
		"downloadPDF": "Download PDF",
		"countries": {
			"Austria": "Austria",
			"Belgium": "Belgium",
			"Bulgaria": "Bulgaria",
			"Croatia": "Croatia",
			"Cyprus": "Cyprus",
			"Czechia": "Czechia",
			"Denmark": "Denmark",
			"Estonia": "Estonia",
			"Finland": "Finland",
			"France": "France",
			"Germany": "Germany",
			"Greece": "Greece",
			"Hungary": "Hungary",
			"Ireland": "Ireland",
			"Irland": "Irland",
			"Latvia": "Latvia",
			"Lithuania": "Lithuania",
			"Luxembourg": "Luxembourg",
			"Malta": "Malta",
			"Netherlands": "Netherlands",
			"Poland": "Poland",
			"Portugal": "Portugal",
			"Romania": "Romania",
			"Slovakia": "Slovakia",
			"Slovenia": "Slovenia",
			"Spain": "Spain",
			"Sweden": "Sweden",
			"Switzerland": "Switzerland",
			"UnitedKingdomofGreatBritain": "United Kingdom of Great Britain",
			"CzechRepublic":"Czech Republic"
        }
	},
	"de": {
		"title1": "Retourenlabel erstellen",
		"title2": "Ihr Rückversand an FDS GmbH.",
		"pleaseEnter": "Bitte tragen Sie Ihre Absenderdetails in die folgenden Felder ein und klicken Sie auf \"Weiter\".",
		"orderNumber": "Auftragsnummer",
		"name": "Vorname und Nachname",
		"country": "Land",
		"address": "Straße Adresse",
		"streetName": "Straße",
		"houseNumber": "Hausnummer",
		"telephoneNumber": "Telefonnummer",
		"zip": "PLZ",
		"city": "Ort",
		"email": "E-Mail",
		"reason": "Rücksendegrund",
		"next": "Weiter",
		"labelBeenCreated": "Das Retourenlabel wurde erstellt.",
		"returnLabel": "Retourenlabel",
		"printLabel": "Drucken Sie Ihr Retourenlabel auf ein DIN A4 Blatt aus und kleben Sie es gut sichtbar auf Ihre Retoure. Bitte entfernen Sie zuvor alle alten Versandlabel und Barcodes.",
		"downloadPDF": "PDF drucken",
		"countries": {
			"Austria": "Österreich",
			"Belgium": "Belgien",
			"Bulgaria": "Bulgarien",
			"Croatia": "Kroatien",
			"Cyprus": "Zypern",
			"Czechia": "Tschechische Republik",
			"Denmark": "Dänemark",
			"Estonia": "Estland",
			"Finland": "Finnland",
			"France": "Frankreich",
			"Germany": "Deutschland",
			"Greece": "Griechenland",
			"Hungary": "Ungarn",
			"Ireland": "Irland",
			"Irland": "Irland",
			"Latvia": "Lettland",
			"Lithuania": "Litauen",
			"Luxembourg": "Luxemburg",
			"Malta": "Malta",
			"Netherlands": "Niederlande",
			"Poland": "Polen",
			"Portugal": "Portugal",
			"Romania": "Rumänien",
			"Slovakia": "Slowakei",
			"Slovenia": "Slowenien",
			"Spain": "Spanien",
			"Sweden": "Schweden",
			"Switzerland": "Schweiz",
			"UnitedKingdomofGreatBritain": "Vereinigtes Königreich Großbritannien",
			"CzechRepublic": "Tschechische Republik"
		}
	}
};
// The default language is English
var lang = "de";
if ('localStorage' in window) {
	var lang = localStorage.getItem('lang') || navigator.language.slice(0, 2);
	if (!Object.keys(arrLang).includes(lang)) lang = 'en';

	$("#language").val(lang);
}
$(document).ready(function () {
	translate(lang);
});
function translate(la) {
	$(".lang").each(function (index, element) {
		$(this).text(arrLang[la][$(this).attr("key")]);
	});

	$('#country option').each(function () {
		if ($(this).attr("key")) {
			$(this).text(arrLang[la]["countries"][$(this).attr("key")]);
        }
    })
}

$("#language").on('change', function () {
	var la = $(this).val();
	translate(la);

	lang = la;
	if ('localStorage' in window) {
		localStorage.setItem('lang', la);
		console.log(localStorage.getItem('lang'));
	}
})

$("#de").click(function (e) {
	lang = $(this).attr("id");
	translate(lang);
	if ('localStorage' in window) {
		localStorage.setItem('lang', lang);
		console.log(localStorage.getItem('lang'));
	}
});
$("#en").click(function (e) {
	lang = $(this).attr("id");
	translate(lang);
	// update localStorage key
	if ('localStorage' in window) {
		localStorage.setItem('lang', lang);
		console.log(localStorage.getItem('lang'));
	}
});