const apiUrl = 'https://localhost:7105/api/EspacioFisico';

function CargarDatos() {
    fetch(apiUrl).then(res => res.json()).then(datos => {
        let filas = '';
        datos.forEach(item => {
            filas += `<tr>
                <td>${item.espacioID || item.id}</td>
                <td>${item.nombre}</td>
                <td>${item.tipo}</td>
                <td>${item.capacidadMaxima}</td>
                <td>
                    <button onclick="CargarParaEditar(${item.espacioID || item.id}, '${item.nombre}', '${item.tipo}', ${item.capacidadMaxima})">Editar</button>
                    <button onclick="Eliminar(${item.espacioID || item.id})">Eliminar</button>
                </td>
            </tr>`;
        });
        document.getElementById('tabla-datos').innerHTML = filas;
    }).catch(err => console.error('Error:', err));
}

function CargarParaEditar(id, nombre, tipo, capacidad) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtNombre').value = nombre;
    document.getElementById('txtTipo').value = tipo;
    document.getElementById('txtCapacidadMaxima').value = capacidad;
}

function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtTipo').value = "";
    document.getElementById('txtCapacidadMaxima').value = "";
}

function Guardar() {
    const id = document.getElementById('txtId').value;
    const obj = {
        espacioID: id == "0" ? 0 : parseInt(id),
        nombre: document.getElementById('txtNombre').value,
        tipo: document.getElementById('txtTipo').value,
        capacidadMaxima: parseInt(document.getElementById('txtCapacidadMaxima').value)
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