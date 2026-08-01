const apiUrl = 'https://localhost:7105/api/Participante';

function CargarDatos() {
    fetch(apiUrl)
        .then(res => res.json())
        .then(datos => {
            let filas = '';
            datos.forEach(item => {
                let fechaCorta = item.fechaNacimiento ? item.fechaNacimiento.split('T')[0] : '';
                filas += `
                    <tr>
                        <td>${item.participanteID || item.id}</td>
                        <td>${item.tutorID}</td>
                        <td>${item.identificacion}</td>
                        <td>${item.nombre}</td>
                        <td>${fechaCorta}</td>
                        <td>${item.alergias || ''}</td>
                        <td>${item.contactoEmergenciaNombre || ''}</td>
                        <td>${item.contactoEmergenciaTelefono || ''}</td>
                        <td>
                            <button onclick="CargarParaEditar(${item.participanteID || item.id}, ${item.tutorID}, '${item.identificacion}', '${item.nombre}', '${fechaCorta}', '${item.alergias || ''}', '${item.contactoEmergenciaNombre || ''}', '${item.contactoEmergenciaTelefono || ''}')">Editar</button>
                            <button onclick="Eliminar(${item.participanteID || item.id})">Eliminar</button>
                        </td>
                    </tr>`;
            });
            document.getElementById('tabla-datos').innerHTML = filas;
        })
        .catch(err => console.error('Error:', err));
}

function CargarParaEditar(id, tutorId, ci, nombre, fecha, alergias, contNombre, contTel) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtTutorID').value = tutorId;
    document.getElementById('txtIdentificacion').value = ci;
    document.getElementById('txtNombre').value = nombre;
    document.getElementById('txtFechaNacimiento').value = fecha;
    document.getElementById('txtAlergias').value = alergias;
    document.getElementById('txtContactoEmergenciaNombre').value = contNombre;
    document.getElementById('txtContactoEmergenciaTelefono').value = contTel;
}

function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtTutorID').value = "";
    document.getElementById('txtIdentificacion').value = "";
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtFechaNacimiento').value = "";
    document.getElementById('txtAlergias').value = "";
    document.getElementById('txtContactoEmergenciaNombre').value = "";
    document.getElementById('txtContactoEmergenciaTelefono').value = "";
}

function Guardar() {
    const id = document.getElementById('txtId').value;

    const objetoData = {
        participanteID: id == "0" ? 0 : parseInt(id),
        tutorID: parseInt(document.getElementById('txtTutorID').value),
        identificacion: document.getElementById('txtIdentificacion').value,
        nombre: document.getElementById('txtNombre').value,
        fechaNacimiento: document.getElementById('txtFechaNacimiento').value,
        alergias: document.getElementById('txtAlergias').value,
        contactoEmergenciaNombre: document.getElementById('txtContactoEmergenciaNombre').value,
        contactoEmergenciaTelefono: document.getElementById('txtContactoEmergenciaTelefono').value
    };

    fetch(id == "0" ? apiUrl : `${apiUrl}/${id}`, {
        method: id == "0" ? 'POST' : 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(objetoData)
    })
        .then(res => {
            if (res.ok) { LimpiarFormulario(); CargarDatos(); }
            else { console.error('Error al guardar Participante'); }
        })
        .catch(err => console.error('Error:', err));
}

function Eliminar(id) {
    fetch(`${apiUrl}/${id}`, { method: 'DELETE' })
        .then(res => { if (res.ok) CargarDatos(); })
        .catch(err => console.error('Error:', err));
}

CargarDatos();