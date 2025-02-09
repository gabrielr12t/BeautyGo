namespace BeautyGo.Domain.Entities.Appointments;

public enum AppointmentStatus
{
    Pending = 1,                // Agendamento pendente (ainda não confirmado)
    Confirmed = 2,              // Agendamento confirmado
    Completed = 3,              // Agendamento concluído (o serviço foi prestado)
    Cancelled = 4,              // Agendamento cancelado (pelo cliente ou pelo salão)
    NoShow = 5,                 // Cliente não compareceu ao agendamento
    Rescheduled = 6,            // Agendamento remarcado (pode ser devido a falta de disponibilidade ou mudança de planos)
    AwaitingConfirmation = 7,   // Agendamento na fila de espera aguardando confirmação
    FeedbackReceived = 8,       // Agendamento finalizado, feedback foi recebido do cliente
}
