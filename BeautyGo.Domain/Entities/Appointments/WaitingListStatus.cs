namespace BeautyGo.Domain.Entities.Appointments;

public enum WaitingListStatus
{
    Waiting = 1,                     // Cliente está na fila de espera aguardando um horário disponível
    Notified = 2,                    // Cliente foi notificado sobre a disponibilidade de horário
    Accepted = 3,                    // Cliente aceitou o novo horário e o agendamento foi feito
    Rejected = 4,                    // Cliente recusou o novo horário
    Timeout = 5,                     // Cliente não respondeu dentro do tempo limite (ex: 4 horas) e foi removido da fila
    ConvertedToAppointment = 6,      // Solicitação da fila de espera foi convertida em um agendamento
    Cancelled = 7,                   // Solicitação foi cancelada antes de se tornar um agendamento
}
