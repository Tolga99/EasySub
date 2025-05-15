window.startStripeCheckout = async function (selectedPlanId, currencyCode, email, promoCode = null) {
    const stripe = Stripe('pk_test_51RKNsaQMSCWUg7Iy36ZcINrGCDJqYwfLEgI43KEinzsmkjsyPPMe95NeKyfhackWctNK17odBTUPM6TYjrfgl3rV00BzpL1THn'); // Mets ici ta vraie clé publique Stripe

    const request = {
        planId: selectedPlanId,
        currencyCode: currencyCode,
        promoCode: promoCode,
        email: email
    };

    const response = await fetch("https://localhost:7237/api/payments/create-checkout-session", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });

    if (!response.ok) {
        const error = await response.text();
        console.error("Erreur API Stripe :", error);
        return;
    }

    const { sessionId } = await response.json();

    // Utilise sessionId pour rediriger vers Stripe Checkout
    const result = await stripe.redirectToCheckout({ sessionId: sessionId });

    if (result.error) {
        console.error("Erreur lors de la redirection vers le checkout Stripe :", result.error.message);
    }
};
