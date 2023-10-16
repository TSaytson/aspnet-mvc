using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using introduction.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace introduction.Controllers
{
  public class PersonController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Details(int id)
    {
      return View();
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(PersonModel person)
    {
      // foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(person))
      // {
      //   string name = descriptor.Name;
      //   object value = descriptor.GetValue(person);
      //   Console.WriteLine("{0}:{1}", name, value);
      // }
      List<PersonModel> people = new List<PersonModel>();
      ModelState.Remove("Id");
      try
      {
        if (!ModelState.IsValid)
        {
          foreach (ModelStateEntry modelState in ViewData.ModelState.Values)
          {
            foreach (ModelError error in modelState.Errors)
            {
              Console.WriteLine(error.ErrorMessage);
            }
          }
          IEnumerable<string> allErrors =
            ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
          foreach (var item in allErrors)
          {
            Console.WriteLine(item);
          }
          return View(person);
        }
        if (HttpContext.Session.GetString("People") != null)
        {
          List<PersonModel> localList =
            JsonSerializer.Deserialize<List<PersonModel>>(HttpContext.Session.GetString("People"));
          people.AddRange(localList);
        }

        person.Id = people.Count + 1;
        people.Add(person);

        HttpContext.Session.SetString("People", JsonSerializer.Serialize(people));
        Console.WriteLine(HttpContext.Session.GetString("People"));

        return View("List", people);

      }
      catch (System.Exception)
      {
        return View();
        throw;
      }
    }

    public ActionResult List()
    {
      if (HttpContext.Session.GetString("People") != null)
      {
        List<PersonModel> people =
          JsonSerializer.Deserialize<List<PersonModel>>(HttpContext.Session.GetString("People"));

        return View("List", people);
      }
      return View("List", new List<PersonModel>());
    }

    public ActionResult Edit(int id)
    {
      List<PersonModel> people =
          JsonSerializer.Deserialize<List<PersonModel>>(HttpContext.Session.GetString("People"));
      if (people.Where(p => p.Id == id).Any())
      {
        PersonModel person = people.Where(p => p.Id == id).FirstOrDefault();
        return View("Create", person);
      }
      return View("Create", new PersonModel());
    }

    [HttpPost]
    public ActionResult Edit(PersonModel person)
    {
      List<PersonModel> people =
        JsonSerializer.Deserialize<List<PersonModel>>(HttpContext.Session.GetString("People"));
      try
      {
        if (people != null)
        {
          if (people.Where(p => p.Id == person.Id).Any())
          {
            people[person.Id - 1] = person;
            HttpContext.Session.SetString("People", JsonSerializer.Serialize(people));
          }
          return View("List", people);
        }
        return RedirectToAction("Index");
      }
      catch (System.Exception)
      {
        return View();
        throw;
      }
    }

    public ActionResult Delete(int id)
    {
      List<PersonModel> people =
        JsonSerializer.Deserialize<List<PersonModel>>(HttpContext.Session.GetString("People"));

      if (people != null && id > 0)
      {
        if (people.Where(p => p.Id == id).Any())
        {
          PersonModel person = people.Where(p => p.Id == id).FirstOrDefault();
          people.Remove(person);
          HttpContext.Session.SetString("People", JsonSerializer.Serialize(people));
        }
        return View("List", people);
      }

      return View("List");
    }

    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        return RedirectToAction("Index");
      }
      catch (System.Exception)
      {
        return View();
        throw;
      }
    }
  }
}