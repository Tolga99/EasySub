window.initPayPalButton = (dotnetHelper) => {
    // Fonction pour vérifier périodiquement si l'élément est prêt
    const checkElementReady = () => {
        const container = document.querySelector("#paypal-button-container");
        if (container) {
            // Si l'élément est trouvé, initialisez le bouton PayPal
            paypal.Buttons({
                style: {
                    shape: "rect",
                    layout: "vertical",
                    color: "gold",
                    label: "paypal",
                },

                createOrder: async function () {
                    const orderId = await dotnetHelper.invokeMethodAsync("CreateOrder");
                    return orderId;
                },

                onApprove: async function (data, actions) {
                    const success = await dotnetHelper.invokeMethodAsync("CaptureOrder", data.orderID);
                    if (!success) {
                        return actions.restart();
                    }

                    document.querySelector("#result-message").innerHTML = "Paiement effectué avec succès !";
                },

                onError: function (err) {
                    console.error("Erreur PayPal : ", err);
                    document.querySelector("#result-message").innerHTML = "Une erreur est survenue.";
                }
            }).render("#paypal-button-container");

            // Arrêter la vérification une fois le bouton PayPal rendu
            clearInterval(intervalId);
        }
    };

    // Vérification toutes les 100 ms pour savoir si l'élément existe
    const intervalId = setInterval(checkElementReady, 100);
};

window.addEventListener('DOMContentLoaded', (event) => {
    if (typeof dotnetHelper !== 'undefined') {
        window.initPayPalButton(dotnetHelper);
    } else {
        console.warn('dotnetHelper is not defined yet.');
        // Tu peux soit attendre qu’il soit défini, soit ignorer, soit déclencher plus tard
    }
});

