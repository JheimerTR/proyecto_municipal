const apiUrl = 'https://localhost:7105/api/Inscripcion';

function CargarDatos() {
    fetch(apiUrl)
        .then(res => res.json())
        .then(datos => {
            let filas = '';
            datos.forEach(item => {
                // Tu C# lo llama FechaInscripcion (con F mayúscula), ajustamos aquí por si acaso
                let fechaCorta = item.fechaInscripcion ? item.fechaInscripcion.split('T')[0] : '';
                filas += `
                    <tr>
                        <td>${item.inscripcionID || item.id}</td>
                        <td>${item.participanteID}</td>
                        <td>${item.cursoID}</td>
                        <td>${fechaCorta}</td>
                        <td>${item.estado}</td>
                        <td>
                            <button onclick="CargarParaEditar(${item.inscripcionID || item.id}, ${item.participanteID}, ${item.cursoID}, '${fechaCorta}', '${item.estado}')">Editar</button>
                            <button onclick="Eliminar(${item.inscripcionID || item.id})">Eliminar</button>
                        </td>
                    </tr>`;
            });
            document.getElementById('tabla-datos').innerHTML = filas;
        })
        .catch(err => console.error('Error:', err));
}

function CargarParaEditar(id, partID, cursoID, fecha, estado) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtParticipanteID').value = partID;
    document.getElementById('txtCursoID').value = cursoID;
    document.getElementById('txtFecha').value = fecha;
    document.getElementById('txtEstado').value = estado;
}

function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtParticipanteID').value = "";
    document.getElementById('txtCursoID').value = "";
    document.getElementById('txtFecha').value = "";
    document.getElementById('txtEstado').value = "Activa"; // Resetea al primer valor
}

function Guardar() {
    const id = document.getElementById('txtId').value;

    // Armamos el objeto con los nombres exactos que tu C# espera
    const objetoData = {
        inscripcionID: id == "0" ? 0 : parseInt(id),
        participanteID: parseInt(document.getElementById('txtParticipanteID').value),
        cursoID: parseInt(document.getElementById('txtCursoID').value),
        fechaInscripcion: document.getElementById('txtFecha').value,
        estado: document.getElementById('txtEstado').value
    };

    fetch(id == "0" ? apiUrl : `${apiUrl}/${id}`, {
        method: id == "0" ? 'POST' : 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(objetoData)
    })
        .then(res => {
            if (res.ok) {
                LimpiarFormulario();
                CargarDatos();
            } else {
                console.error('Error al guardar. Revisa la consola o tu base de datos.');
            }
        })
        .catch(err => console.error('Error de red:', err));
}

function Eliminar(id) {
    fetch(`${apiUrl}/${id}`, { method: 'DELETE' })
        .then(res => { if (res.ok) CargarDatos(); })
        .catch(err => console.error('Error:', err));
}

CargarDatos();