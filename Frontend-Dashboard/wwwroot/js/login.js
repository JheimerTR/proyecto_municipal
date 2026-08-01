document.getElementById("formularioLogin").addEventListener("submit", async function (event) {
    event.preventDefault(); // Evita que el formulario recargue la página

    // Capturamos los datos ingresados
    const usuario = document.getElementById("usuarioRed").value;
    const pass = document.getElementById("contrasena").value;
    const divError = document.getElementById("mensajeError");

    // Limpiamos los errores visuales de intentos anteriores
    divError.style.display = "none";
    divError.innerText = "";

    // Armamos el JSON estructurado para el [FromBody] de tu controlador
    const payload = {
        usuarioRed: usuario,
        contrasena: pass
    };

    try {
        // Asegúrate de que este puerto (7105) coincida con el que te dio Swagger
        const respuesta = await fetch("https://localhost:7105/api/Usuario/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        if (respuesta.ok) {
            const datosUsuario = await respuesta.json();

            // Login exitoso
            alert("Conexión exitosa. Bienvenido al sistema: " + datosUsuario.nombreCompleto);

            localStorage.setItem("rolUsuario", datosUsuario.rol);

            localStorage.setItem("nombreUsuario", datosUsuario.nombreCompleto);

            window.location.href = "/Home/Index";
            // Aquí puedes colocar la ruta a tu menú principal para redirigirlo
            // window.location.href = "/Home/Index"; 
        } else {
            // El backend devolvió HTTP 401, encendemos tu div rojo
            divError.innerText = "Credenciales incorrectas o usuario inactivo.";
            divError.style.display = "block";
        }
    } catch (error) {
        // Falla de red o servidor caído
        divError.innerText = "Error crítico de conexión con el servidor.";
        divError.style.display = "block";
        console.error("Detalle del error:", error);
    }
});