const apiUrl = 'https://localhost:7105/api/Asistencia';

function CargarDatos() {
    fetch(apiUrl).then(res => res.json()).then(datos => {
        let filas = '';
        datos.forEach(item => {
            let fechaCorta = item.fecha ? item.fecha.split('T')[0] : '';
            filas += `<tr>
                <td>${item.asistenciaID || item.id}</td>
                <td>${item.inscripcionID}</td>
                <td>${fechaCorta}</td>
                <td>${item.estado}</td>
                <td>${item.registradoPor}</td>
                <td>
                    <button onclick="CargarParaEditar(${item.asistenciaID || item.id}, ${item.inscripcionID}, '${fechaCorta}', '${item.estado}', ${item.registradoPor})">Editar</button>
                    <button onclick="Eliminar(${item.asistenciaID || item.id})">Eliminar</button>
                </td>
            </tr>`;
        });
        document.getElementById('tabla-datos').innerHTML = filas;
    }).catch(err => console.error('Error:', err));
}
function CargarParaEditar(id, insID, fecha, estado, registradoPor) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtInscripcionID').value = insID;
    document.getElementById('txtFecha').value = fecha;
    document.getElementById('txtEstado').value = estado;
    document.getElementById('txtRegistradoPor').value = registradoPor;
}
function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtInscripcionID').value = "";
    document.getElementById('txtFecha').value = "";
    document.getElementById('txtEstado').value = "Presente";
    document.getElementById('txtRegistradoPor').value = "";
}
function Guardar() {
    const id = document.getElementById('txtId').value;
    const obj = {
        asistenciaID: id == "0" ? 0 : parseInt(id),
        inscripcionID: parseInt(document.getElementById('txtInscripcionID').value),
        fecha: document.getElementById('txtFecha').value,
        estado: document.getElementById('txtEstado').value,
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