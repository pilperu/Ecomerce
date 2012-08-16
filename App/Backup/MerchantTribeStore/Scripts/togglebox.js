function toggleBox(){
          $(this).next().slideToggle('fast');
          var imgs = $(this).children('img');                   
          imgs.toggleClass('collapsed');
          imgs.toggleClass('expanded');
          imgs.filter('img.collapsed').attr('src', 'images/plus.png')
          .end().filter('img.expanded').attr('src', 'images/minus.png');        
      }