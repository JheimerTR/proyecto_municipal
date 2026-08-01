const apiUrl = 'https://localhost:7105/api/Tutor';

// 1. FUNCIÓN PARA CARGAR LA TABLA (¡Esta era la que faltaba!)
function CargarDatos() {
    fetch(apiUrl)
        .then(respuesta => respuesta.json())
        .then(datos => {
            let filas = '';
            datos.forEach(item => {
                const id = item.tutorID || item.TutorID;
                const nombre = item.nombre || item.Nombre || '';
                const identificacion = item.identificacion || item.Identificacion || '';
                const telefono = item.telefono || item.Telefono || '';
                const email = item.email || item.Email || '';

                filas += `
                    <tr>
                        <td>${id}</td>
                        <td>${nombre}</td>
                        <td>${identificacion}</td>
                        <td>${telefono}</td>
                        <td>${email}</td>
                        <td>
                            <button type="button" onclick="CargarParaEditar(${id}, '${nombre}', '${identificacion}', '${telefono}', '${email}')">Editar</button>
                            <button type="button" onclick="Eliminar(${id})">Eliminar</button>
                        </td>
                    </tr>
                `;
            });
            document.getElementById('tabla-datos').innerHTML = filas;
        })
        .catch(error => console.error('Error al cargar datos:', error));
}

// 2. FUNCIÓN PARA GUARDAR / ACTUALIZAR (Versión única y blindada)
function Guardar(event) {
    if (event) event.preventDefault();

    const elId = document.getElementById('txtId');
    const elNombre = document.getElementById('txtNombre');
    const elIdentificacion = document.getElementById('txtIdentificacion');
    const elTelefono = document.getElementById('txtTelefono');
    const elEmail = document.getElementById('txtEmail');

    // Validamos que los inputs existan en el HTML
    if (!elId) { alert("Error: No se encontró el input 'txtId'"); return; }
    if (!elNombre) { alert("Error: No se encontró el input 'txtNombre'"); return; }
    if (!elIdentificacion) { alert("Error: No se encontró el input 'txtIdentificacion'"); return; }
    if (!elTelefono) { alert("Error: No se encontró el input 'txtTelefono'"); return; }
    if (!elEmail) { alert("Error: No se encontró el input 'txtEmail'"); return; }

    const id = elId.value;
    const nombre = elNombre.value;
    const identificacion = elIdentificacion.value;
    const telefono = elTelefono.value;
    const email = elEmail.value;

    const objetoData = {
        tutorID: id === "0" ? 0 : parseInt(id),
        nombre: nombre,
        identificacion: identificacion,
        telefono: telefono,
        email: email
    };

    const metodo = id === "0" ? 'POST' : 'PUT';
    const urlDestino = id === "0" ? apiUrl : `${apiUrl}/${id}`;

    fetch(urlDestino, {
        method: metodo,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(objetoData)
    })
        .then(respuesta => {
            if (respuesta.ok) {
                LimpiarFormulario();
                CargarDatos();
            } else {
                alert('Rechazado por la base de datos. ¿Carnet duplicado? Revisa la consola (F12)');
            }
        })
        .catch(error => console.error('Error de red:', error));
}

// 3. FUNCIÓN PARA CARGAR DATOS EN EL FORMULARIO
function CargarParaEditar(id, nombre, identificacion, telefono, email) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtNombre').value = nombre;
    document.getElementById('txtIdentificacion').value = identificacion;
    document.getElementById('txtTelefono').value = telefono;
    document.getElementById('txtEmail').value = email;
}

// 4. FUNCIÓN PARA LIMPIAR EL FORMULARIO
function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtIdentificacion').value = "";
    document.getElementById('txtTelefono').value = "";
    document.getElementById('txtEmail').value = "";
}

// 5. FUNCIÓN PARA ELIMINAR
function Eliminar(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este registro?")) return;

    fetch(`${apiUrl}/${id}`, {
        method: 'DELETE'
    })
        .then(respuesta => {
            if (respuesta.ok) {
                CargarDatos();
            } else {
                console.error('Error al eliminar en la base de datos.');
            }
        })
        .catch(error => console.error('Error:', error));
}

// 6. LLAMADA INICIAL (Dibuja la tabla al abrir la página)
CargarDatos();