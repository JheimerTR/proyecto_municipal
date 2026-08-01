const apiUrl = 'https://localhost:7105/api/Horario';

function CargarDatos() {
    fetch(apiUrl).then(res => res.json()).then(datos => {
        let filas = '';
        datos.forEach(item => {
            // Protección contra mayúsculas del backend de C#
            const id = item.horarioID || item.HorarioID || item.id || 0;
            const dia = item.diaSemana || item.DiaSemana || '';
            const inicio = item.horaInicio || item.HoraInicio || '';
            const fin = item.horaFin || item.HoraFin || '';

            filas += `<tr>
                <td>${id}</td>
                <td>${dia}</td>
                <td>${inicio}</td>
                <td>${fin}</td>
                <td>
                    <button class="btn btn-sm btn-primary" type="button" onclick="CargarParaEditar(${id}, '${dia}', '${inicio}', '${fin}')">Editar</button>
                    <button class="btn btn-sm btn-danger" type="button" onclick="Eliminar(${id})">Eliminar</button>
                </td>
            </tr>`;
        });
        document.getElementById('tabla-datos').innerHTML = filas;
    }).catch(err => console.error('Error:', err));
}

function CargarParaEditar(id, dia, inicio, fin) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtDiaSemana').value = dia;

    // Si la hora viene completa de C# (ej: "14:30:00"), recortamos a "14:30" para que el input type="time" la reconozca
    document.getElementById('txtHoraInicio').value = inicio.substring(0, 5);
    document.getElementById('txtHoraFin').value = fin.substring(0, 5);
}

function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtHoraInicio').value = "";
    document.getElementById('txtHoraFin').value = "";
}

function Guardar(event) {
    if (event) event.preventDefault();

    const id = document.getElementById('txtId').value;
    let inicio = document.getElementById('txtHoraInicio').value;
    let fin = document.getElementById('txtHoraFin').value;

    if (!inicio || !fin) {
        alert("Por favor ingrese las horas de inicio y fin.");
        return;
    }

    // Inyectamos los segundos obligatorios para C#
    if (inicio.length === 5) inicio += ":00";
    if (fin.length === 5) fin += ":00";

    const obj = {
        horarioID: id == "0" ? 0 : parseInt(id),
        diaSemana: document.getElementById('txtDiaSemana').value,
        horaInicio: inicio,
        horaFin: fin
    };

    fetch(id == "0" ? apiUrl : `${apiUrl}/${id}`, {
        method: id == "0" ? 'POST' : 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(obj)
    }).then(res => {
        if (res.ok) {
            LimpiarFormulario();
            CargarDatos();
        } else {
            console.error('El backend rechazó la petición.');
            alert("Error al guardar. Revisa la consola.");
        }
    })
        .catch(err => console.error('Error:', err));
}

function Eliminar(id) {
    if (!confirm("¿Seguro que deseas eliminar este horario?")) return;

    fetch(`${apiUrl}/${id}`, { method: 'DELETE' })
        .then(res => { if (res.ok) CargarDatos(); })
        .catch(err => console.error('Error:', err));
}

CargarDatos();