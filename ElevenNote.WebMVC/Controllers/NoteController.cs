using ElevenNote.Services.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebMVC.Controllers;

[Authorize]
public class NoteController : Controller
{
    private readonly INoteService _noteServices;
    public NoteController(INoteService noteService)
    {
        _noteServices = noteService;
    }

    public async Task<IActionResult> Index()
    {
        var notes = await _noteServices.GetAllNotesAsync();
        return View(notes);
    }
}