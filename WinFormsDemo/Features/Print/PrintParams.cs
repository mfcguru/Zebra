using POS.Printers;

namespace WinFormsDemo.Features.Print;

public record PrintParams(Mode Mode, string Content, string Identifier, int? port = null);
