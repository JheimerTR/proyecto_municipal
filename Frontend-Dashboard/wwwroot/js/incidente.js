const apiUrl = 'https://localhost:7105/api/Incidente';

function CargarDatos() {
    fetch(apiUrl).then(res => res.json()).then(datos => {
        let filas = '';
        datos.forEach(item => {
            filas += `<tr>
                <td>${item.incidenteID || item.id}</td>
                <td>${item.participanteID}</td>
                <td>${item.cursoID}</td>
                <td>${item.fecha}</td>
                <td>${item.descripcion}</td>
                <td>${item.registradoPor}</td>
                <td>
                    <button onclick="CargarParaEditar(${item.incidenteID || item.id}, ${item.participanteID}, ${item.cursoID}, '${item.fecha}', '${item.descripcion}', ${item.registradoPor})">Editar</button>
                    <button onclick="Eliminar(${item.incidenteID || item.id})">Eliminar</button>
                </td>
            </tr>`;
        });
        document.getElementById('tabla-datos').innerHTML = filas;
    }).catch(err => console.error('Error:', err));
}
function CargarParaEditar(id, parID, curID, fecha, desc, registradoPor) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtParticipanteID').value = parID;
    document.getElementById('txtCursoID').value = curID;
    document.getElementById('txtFecha').value = fecha;
    document.getElementById('txtDescripcion').value = desc;
    document.getElementById('txtRegistradoPor').value = registradoPor;
}
function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtParticipanteID').value = "";
    document.getElementById('txtCursoID').value = "";
    document.getElementById('txtFecha').value = "";
    document.getElementById('txtDescripcion').value = "";
    document.getElementById('txtRegistradoPor').value = "";
}
function Guardar() {
    const id = document.getElementById('txtId').value;
    const obj = {
        incidenteID: id == "0" ? 0 : parseInt(id),
        participanteID: parseInt(document.getElementById('txtParticipanteID').value),
        cursoID: parseInt(document.getElementById('txtCursoID').value),
        fecha: document.getElementById('txtFecha').value,
        descripcion: document.getElementById('txtDescripcion').value,
        registradoPor: parseInt(document.getElementById('txtRegistradoPor').value)
    };
    fetch(id == "0" ? apiUrl : `${apiUrl}/${id}`, {
        method: id == "0" ? 'POST' : 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(obj)
    }).then(res => { if (res.ok) { LimpiarFormulario(); CargarDatos(); } })
        .catch(err => console.error('Error:', err));
}
function Eliminar(id) { fetch(`${apiUrl}/${id}`, { method: 'DELETE' }).then(res => { if (res.ok) CargarDatos(); }); }
CargarDatos();